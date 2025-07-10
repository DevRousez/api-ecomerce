using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Models;
using Microsoft.EntityFrameworkCore;
using Api_comerce.Reports.ventas;
using Api_comerce.Services.correos;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace Api_comerce.Services.Checkout
{
    public class CheckoutService : ICheckoutService
    {
        private readonly AppDbContext _context;
        private readonly ICorreoSevice _correoService;

        public CheckoutService(AppDbContext context, ICorreoSevice correoService)
        {
            _context = context;
            _correoService = correoService; 
        }

        public async Task<int> CrearOrdenAsync(CheckoutRequest dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("La orden no contiene productos.");


            var direccion = await _context.AccountsDirecciones.FindAsync(dto.AccountsDireccionId);
            if (direccion == null)
                throw new ArgumentException("Dirección no válida.");

            DatosFacturacion? datosFacturacion = null;
            if (dto.DatosFacturacionId.HasValue)
            {
                datosFacturacion = await _context.DatosFacturacion.FindAsync(dto.DatosFacturacionId.Value);
                if (datosFacturacion == null)
                    throw new ArgumentException("Datos de facturación no válidos.");
            }


            var orden = new Orden
            {
                AccountsDireccionId = dto.AccountsDireccionId,
                DatosFacturacionId = dto.DatosFacturacionId,
                NombreCompleto= dto.NombreCompleto,
                MetodoPago = dto.MetodoPago.Tipo,
                Correo= dto.Correo,
                Telefono= "0",
                Direccion="",
                Estado = "Pendiente",
                FechaCreacion = DateTime.Now
            };

            foreach (var item in dto.Items)
            {
                orden.Detalles.Add(new OrdenDetalle
                {
                    ProductoEmpaqueId = item.ProductEmpaqueId,
                    Cantidad = item.Quantity,
                    Precio = item.Price,
                 
                });
            }

            _context.Orden.Add(orden);
            await _context.SaveChangesAsync();
            int idOrden= orden.Id;

            var ordenReporte = await _context.Orden
 .Include(o => o.Detalles)
     .ThenInclude(d => d.ProductoEmpaque)
         .ThenInclude(pe => pe.Producto)
         .Include(o => o.Detalles)
     .ThenInclude(d => d.ProductoEmpaque)
         .ThenInclude(pe => pe.Producto)
             .ThenInclude(p => p.ProductosCaracteristicas)
 // Incluye más si necesitas, por ejemplo Marca, Linea, etc.
 .Include(o => o.Detalles)
     .ThenInclude(d => d.ProductoEmpaque)
         .ThenInclude(pe => pe.Producto)
             .ThenInclude(p => p.MarcaProducto)
 .Include(o => o.Detalles)
     .ThenInclude(d => d.ProductoEmpaque)
        .ThenInclude(d => d.ImagenProducto)
 // También incluye la dirección y datos facturación si los usas en el reporte
 .Include(o => o.AccountsDireccion)
 .Include(o => o.DatosFacturacion)
 .FirstOrDefaultAsync(o => o.Id == idOrden);

            var pdfBytes = NotaVentaPdfCorreo.Generar(ordenReporte);
            //var emailService = new CorreoSevice();
            //emailService.EnviarNotaVentaAsync(orden.Correo, pdfBytes, orden.Id);


            _correoService.EnviarNotaVentaAsync(orden.Correo, pdfBytes, orden.Id);

            return idOrden;
        }
        public async Task<OrdenDto?> GetOrdenByIdAsync(int ordenId)
        {
            var orden = await _context.Orden
     .Include(o => o.AccountsDireccion)
     .Include(o => o.DatosFacturacion)
     .Include(o => o.Detalles)
         .ThenInclude(d => d.ProductoEmpaque)
             .ThenInclude(pe => pe.Producto)
                 .ThenInclude(p => p.ProductosCaracteristicas)
     .Include(o => o.Detalles)
         .ThenInclude(d => d.ProductoEmpaque)
             .ThenInclude(pe => pe.Empaque)
     .Include(o => o.Detalles)
         .ThenInclude(d => d.ProductoEmpaque)
             .ThenInclude(pe => pe.ImagenProducto)
     .FirstOrDefaultAsync(o => o.Id == ordenId);

            if (orden == null) return null;

            return new OrdenDto
            {
                Id = orden.Id,
                Direccion = new AccountDireccionDto
                {
                    AccountId = orden.AccountsDireccion.Id,
                    Calle = orden.AccountsDireccion.Calle,
                    Ciudad = orden.AccountsDireccion.Ciudad,
                    Estado = orden.AccountsDireccion.Estado,
                    CodigoPostal = orden.AccountsDireccion.CodigoPostal,
                    Pais = orden.AccountsDireccion.Pais,
                    Telefono = orden.AccountsDireccion.Telefono,
                    Tipo = orden.AccountsDireccion.Tipo,
                    EsPredeterminada = orden.AccountsDireccion.EsPredeterminada
                },
                DatosFacturacion = orden.DatosFacturacion != null ? new DatosFacturacionDto
                {
                    Id = orden.DatosFacturacion.Id,
                    RazonSocial = orden.DatosFacturacion.RazonSocial,
                    RFC = orden.DatosFacturacion.RFC,
                    UsoCFDI = orden.DatosFacturacion.UsoCFDI,
                    RegimenFiscal = orden.DatosFacturacion.RegimenFiscal
                } : null,
                MetodoPago = orden.MetodoPago,
                Estado = orden.Estado,
                FechaCreacion = orden.FechaCreacion,
                Detalles = orden.Detalles.Select(d => new OrdenDetalleDto
                {
                    ProductoEmpaqueId = d.ProductoEmpaqueId,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    ProductoEmpaque = new ProductoEmpaqueOrdenDto
                    {
                        ProductoEmpaqueId = d.ProductoEmpaque.Id,
                        ProductoId = d.ProductoEmpaque.ProductoId,
                        EmpaqueId = d.ProductoEmpaque.EmpaqueId,
                        Codigo = d.ProductoEmpaque.Codigo ?? "NO DATO",
                        PVenta = d.ProductoEmpaque.PVenta ?? 0,
                        PCompra = d.ProductoEmpaque.PCompra ?? 0,
                        Activo = d.ProductoEmpaque.Activo,
                        Descuento = (float?)(d.ProductoEmpaque.Descuento ?? 0),

                        Producto = d.ProductoEmpaque.Producto != null ? new ProductoOrdenDto
                        {
                            ProductId = d.ProductoEmpaque.Producto.Id,
                            ProductoSatId = d.ProductoEmpaque.Producto.ProductoSatId,
                            NombreProducto = d.ProductoEmpaque.Producto.NombreProducto ?? "NO DATO",
                            Slug = d.ProductoEmpaque.Producto.Slug ?? "no-slug",
                            Descripcion = d.ProductoEmpaque.Producto.Descripcion ?? "NO DATO",
                            Prefijo = d.ProductoEmpaque.Producto.Prefijo,
                            DescripcionBreve = d.ProductoEmpaque.Producto.DescripcionBreve,
                            Rating = d.ProductoEmpaque.Producto.Rating,
                            Acumulador = d.ProductoEmpaque.Producto.Acumulador,
                            ProductoIdAcumulador = d.ProductoEmpaque.Producto.ProductoIdAcumulador,
                            CategoriaTipo = d.ProductoEmpaque.Producto.CategoriaTipo,
                            Linea = d.ProductoEmpaque.Producto.Linea != null
                            ? new LineaDto
                            {
                                Id = d.ProductoEmpaque.Producto.Linea.Id,
                                Linea = d.ProductoEmpaque.Producto.Linea.Linea,
                                Slug = d.ProductoEmpaque.Producto.Linea.Slug
                            }
                            : null,
                            MarcaProducto = d.ProductoEmpaque.Producto.MarcaProducto != null
                            ? new MarcaProductoDto
                            {
                                Id = d.ProductoEmpaque.Producto.MarcaProducto.Id,
                                Marca = d.ProductoEmpaque.Producto.MarcaProducto.Marca,
                                Slug = d.ProductoEmpaque.Producto.MarcaProducto.Slug
                            }

                            : null,
                            ProductosCaracteristicas = d.ProductoEmpaque.Producto.ProductosCaracteristicas != null
                        ? d.ProductoEmpaque.Producto.ProductosCaracteristicas.Select(c => new ProductosCaracteristicasDTO
                        {
                            Id = c.Id,
                            ProductoId = c.Productoid,
                            Nombre = c.Nombre,
                            Descripcion = c.Descripcion,
                            FechaCreado = c.FechaCreado
                        }).ToList()
                        : new List<ProductosCaracteristicasDTO>()

                        } : null,
                        Empaque = d.ProductoEmpaque.Empaque != null ? new EmpaqueDto
                        {
                            Id = d.ProductoEmpaque.Empaque.Id,
                            Empaque = d.ProductoEmpaque.Empaque.Empaque ?? "NO DATO",
                            Contenido = d.ProductoEmpaque.Empaque.Contenido,
                            CodigoEmpaque = d.ProductoEmpaque.Empaque.CodigoEmpaque ?? "NO DATO"
                        } : null,
                        ImagenProducto = d.ProductoEmpaque.ImagenProducto.Any()
                            ? d.ProductoEmpaque.ImagenProducto.Select(img => new ImageDto
                            {
                                Id = img.Id,
                                Name = System.IO.Path.GetFileName(img.Url),
                                Url = img.Url,
                                Width = (int)img.Width,
                                Height = (int)img.Height,
                                Formats = new FormatDto()
                            }).ToList()
                            : new List<ImageDto>()
                    }
                }).ToList()
            };
        }

        public async Task<bool> MarcarOrdenComoPagadaAsync(int ordenId)
        {
            var orden = await _context.Orden.FindAsync(ordenId);
            if (orden == null) return false;

            orden.Estado = "Pagada";
            await _context.SaveChangesAsync();

            return true;
        }


      
        public async Task<List<OrdenDto>> GetOrdenesPorUsuarioAsync(int accountId)
        {
            var ordenes = await _context.Orden
                .Include(o => o.AccountsDireccion)
                .Include(o => o.DatosFacturacion)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.ProductoEmpaque)
                        .ThenInclude(pe => pe.Producto)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.ProductoEmpaque)
                        .ThenInclude(pe => pe.Empaque)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.ProductoEmpaque)
                        .ThenInclude(pe => pe.ImagenProducto)
                 .Include(o => o.Detalles)
                    .ThenInclude(d => d.ProductoEmpaque)
                        .ThenInclude(pe => pe.Producto)
                            .ThenInclude(pe => pe.ProductosCaracteristicas)
                .Where(o => o.AccountsDireccion.AccountId == accountId)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return ordenes.Select(orden => new OrdenDto
            {
                Id = orden.Id,
                Direccion = new AccountDireccionDto
                {
                    AccountId = orden.AccountsDireccion.Id,
                    Calle = orden.AccountsDireccion.Calle,
                    Ciudad = orden.AccountsDireccion.Ciudad,
                    Estado = orden.AccountsDireccion.Estado,
                    CodigoPostal = orden.AccountsDireccion.CodigoPostal,
                    Pais = orden.AccountsDireccion.Pais,
                    Telefono = orden.AccountsDireccion.Telefono,
                    Tipo = orden.AccountsDireccion.Tipo,
                    EsPredeterminada = orden.AccountsDireccion.EsPredeterminada
                },
                DatosFacturacion = orden.DatosFacturacion != null ? new DatosFacturacionDto
                {
                    Id = orden.DatosFacturacion.Id,
                    RazonSocial = orden.DatosFacturacion.RazonSocial,
                    RFC = orden.DatosFacturacion.RFC,
                    UsoCFDI = orden.DatosFacturacion.UsoCFDI,
                    RegimenFiscal = orden.DatosFacturacion.RegimenFiscal
                } : null,
                MetodoPago = orden.MetodoPago,
                Estado = orden.Estado,
                FechaCreacion = orden.FechaCreacion,
                Detalles = orden.Detalles.Select(d => new OrdenDetalleDto
                {
                    ProductoEmpaqueId = d.ProductoEmpaqueId,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    ProductoEmpaque = new ProductoEmpaqueOrdenDto
                    {
                        ProductoEmpaqueId = d.ProductoEmpaque.Id,
                        ProductoId = d.ProductoEmpaque.ProductoId,
                        EmpaqueId = d.ProductoEmpaque.EmpaqueId,
                        Codigo = d.ProductoEmpaque.Codigo ?? "NO DATO",
                        PVenta = d.ProductoEmpaque.PVenta ?? 0,
                        PCompra = d.ProductoEmpaque.PCompra ?? 0,
                        Activo = d.ProductoEmpaque.Activo,
                        Descuento = (float?)(d.ProductoEmpaque.Descuento ?? 0),
                       
                        Producto = d.ProductoEmpaque.Producto != null ? new ProductoOrdenDto
                        {
                            ProductId = d.ProductoEmpaque.Producto.Id,
                            ProductoSatId=  d.ProductoEmpaque.Producto.ProductoSatId,
                            NombreProducto = d.ProductoEmpaque.Producto.NombreProducto ?? "NO DATO",
                            Slug = d.ProductoEmpaque.Producto.Slug ?? "no-slug",
                            Descripcion = d.ProductoEmpaque.Producto.Descripcion ?? "NO DATO",
                            Prefijo =  d.ProductoEmpaque.Producto.Prefijo,
                            DescripcionBreve= d.ProductoEmpaque.Producto.DescripcionBreve,
                            Rating = d.ProductoEmpaque.Producto.Rating,
                            Acumulador = d.ProductoEmpaque.Producto.Acumulador,
                            ProductoIdAcumulador = d.ProductoEmpaque.Producto.ProductoIdAcumulador,
                            CategoriaTipo = d.ProductoEmpaque.Producto.CategoriaTipo,
                            Linea = d.ProductoEmpaque.Producto.Linea != null
                            ? new LineaDto
                            {
                                Id = d.ProductoEmpaque.Producto.Linea.Id,
                                Linea = d.ProductoEmpaque.Producto.Linea.Linea,
                                Slug = d.ProductoEmpaque.Producto.Linea.Slug
                            }
                            : null,
                            MarcaProducto = d.ProductoEmpaque.Producto.MarcaProducto != null
                            ? new MarcaProductoDto
                                {
                                    Id = d.ProductoEmpaque  .Producto.MarcaProducto.Id,
                                    Marca = d.ProductoEmpaque.Producto.MarcaProducto.Marca,
                                    Slug = d.ProductoEmpaque.Producto.MarcaProducto.Slug
                                }
                            
                            : null,
                            ProductosCaracteristicas = d.ProductoEmpaque.Producto.ProductosCaracteristicas != null
                        ? d.ProductoEmpaque.Producto.ProductosCaracteristicas.Select(c => new ProductosCaracteristicasDTO
                        {
                            Id = c.Id,
                            ProductoId = c.Productoid,
                            Nombre = c.Nombre,
                            Descripcion = c.Descripcion,
                            FechaCreado = c.FechaCreado
                        }).ToList()
                        : new List<ProductosCaracteristicasDTO>()

                        } : null,
                        Empaque = d.ProductoEmpaque.Empaque != null ? new EmpaqueDto
                        {
                            Id = d.ProductoEmpaque.Empaque.Id,
                            Empaque = d.ProductoEmpaque.Empaque.Empaque ?? "NO DATO",
                            Contenido = d.ProductoEmpaque.Empaque.Contenido,
                            CodigoEmpaque = d.ProductoEmpaque.Empaque.CodigoEmpaque ?? "NO DATO"
                        } : null,
                        ImagenProducto = d.ProductoEmpaque.ImagenProducto.Any()
                            ? d.ProductoEmpaque.ImagenProducto.Select(img => new ImageDto
                            {
                                Id = img.Id,
                                Name = System.IO.Path.GetFileName(img.Url),
                                Url = img.Url,
                                Width = (int)img.Width,
                                Height = (int)img.Height,
                                Formats = new FormatDto()
                            }).ToList()
                            : new List<ImageDto>()
                    }
                }).ToList()
            }).ToList();
        }

        public async Task<bool> ReenviarNotaVentaCorreoAsync(int ordenId)
        {
            var orden = await _context.Orden
     .Include(o => o.Detalles)
         .ThenInclude(d => d.ProductoEmpaque)
             .ThenInclude(pe => pe.Producto)
             .Include(o => o.Detalles)
         .ThenInclude(d => d.ProductoEmpaque)
             .ThenInclude(pe => pe.Producto)
                 .ThenInclude(p => p.ProductosCaracteristicas)
     // Incluye más si necesitas, por ejemplo Marca, Linea, etc.
     .Include(o => o.Detalles)
         .ThenInclude(d => d.ProductoEmpaque)
             .ThenInclude(pe => pe.Producto)
                 .ThenInclude(p => p.MarcaProducto)
     .Include(o => o.Detalles)
         .ThenInclude(d => d.ProductoEmpaque)
            .ThenInclude(d => d.ImagenProducto)
     // También incluye la dirección y datos facturación si los usas en el reporte
     .Include(o => o.AccountsDireccion)
     .Include(o => o.DatosFacturacion)
     .FirstOrDefaultAsync(o => o.Id == ordenId);

            if (orden == null)
                return false;

            var pdfBytes = NotaVentaPdfCorreo.Generar(orden);

            await _correoService.EnviarNotaVentaAsync(orden.Correo, pdfBytes, orden.Id);

            return true;
        }


    }
}
