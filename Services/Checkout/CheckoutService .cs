using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Models;
using Microsoft.EntityFrameworkCore;
using static Api_comerce.Models.Orden;

namespace Api_comerce.Services.Checkout
{
    public class CheckoutService : ICheckoutService
    {
        private readonly AppDbContext _context;

        public CheckoutService(AppDbContext context)
        {
            _context = context;
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
                    ProductoId = item.ProductEmpaqueId,
                    Cantidad = item.Quantity,
                    Precio = item.Price,
                 
                });
            }

            _context.Orden.Add(orden);
            await _context.SaveChangesAsync();

            return orden.Id;
        }
        public async Task<OrdenDto?> GetOrdenByIdAsync(int ordenId)
        {
            var orden = await _context.Orden
                .Include(o => o.AccountsDireccion)
                .Include(o => o.DatosFacturacion)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.ProductosEmpaque)
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
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio
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


    }
}
