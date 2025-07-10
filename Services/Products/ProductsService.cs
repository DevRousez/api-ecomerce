using Api_comerce.Data;
using Api_comerce.Dtos;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Api_comerce.Models;
using System;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Api_comerce.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductsService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<ProductoEcommerceDto>> GetAllProductosAsync()
        {
            var baseUrl = "";

            var defaultBadges = new List<MarcaProductoDto>
    {
        new MarcaProductoDto { Id = 0, Marca = "NO DATO", Slug = "no-dato" }
    };

            var productosRaw = await _context.ProductosEmpaque
     .Include(pe => pe.Producto)
         .ThenInclude(p => p.Linea)
     .Include(pe => pe.Producto)
         .ThenInclude(p => p.MarcaProducto)
     .Include(pe => pe.Empaque)
         .ThenInclude(e => e.UnidadSAT)
     .Include(pe => pe.ImagenProducto) // <- esta línea es la clave nueva
      .Include(pe => pe.Producto)
                  .ThenInclude(p => p.ProductosCaracteristicas)
     .ToListAsync();
              

            // SEGUNDA PARTE: Creamos los DTOs en memoria
            var productos = productosRaw.Select(p => new ProductoEcommerceDto
            {
                ProductoEmpaqueId = p.Id,
                Name = $"{p?.Codigo ?? "SIN-COD"} - {p.Producto.NombreProducto ?? "NO DATO"} - {p?.Empaque?.Empaque ?? "SIN UNIDAD"}",
                Featured = false,
                Price = p?.PVenta ?? 0,
                SalePrice = null,
                OnSale = false,
                Slug = p.Producto.Slug ?? "no-dato",
                IsStock = true,
                RatingCount = p.Producto.Rating ?? 0,
                Description = p.Producto.Descripcion ?? "NO DATO",
                ShortDescription = p.Producto.DescripcionBreve ?? "NO DATO",
                CategoriaTipo = p.Producto.CategoriaTipo ?? "Default",

                CreatedAt = p.Producto.CreatedAt,
                UpdatedAt = p.Producto.UpdatedAt,

                Sizes = new List<EmpaqueDto>
                    {
                        new EmpaqueDto
                        {
                            Id = p.EmpaqueId,
                            Empaque = p.Empaque.Empaque,
                            Contenido = p.Empaque.  Contenido,
                            Sincronizado = p.Empaque.Sincronizado,
                            CodigoEmpaque = p.Empaque.CodigoEmpaque,
                            Codigo = p.Codigo ?? "NO DATO",
                            PCompra = p.PCompra ?? 0,
                            PVenta = p.PVenta ?? 0,
                            Descuento = p.Descuento ?? 0,
                            Activo = p.Activo ?? false,
                            FechaCreado = p.Empaque.FechaCreado,
                            UnidadSat = p.Empaque?.UnidadSAT != null ? new UnidadSatDto
                            {
                                Id = p.Empaque.UnidadSAT.Id,
                                ClaveUnidad = p.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                                UnidadSat = p.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
                            } : null
                        }
                    },

                Colors = new List<string> { "#eb7b8b", "#000000", "#927764" }, // Estático por ahora

                Badges = p.Producto.MarcaProducto != null
                    ? new List<MarcaProductoDto>
                    {
                        new MarcaProductoDto
                        {
                            Id = p.Producto.MarcaProducto.Id,
                            Marca = p.Producto.MarcaProducto.Marca ?? "NO DATO",
                            Slug = p.Producto.MarcaProducto.Slug ?? "NO DATO"
                        }
                    }
                    : defaultBadges,

                Images = p.ImagenProducto?.Any() == true
                        ? p.ImagenProducto.Select(img => new ImageDto
                        {
                            Id = img.Id,
                            Name = System.IO.Path.GetFileName(img.Url),
                            Url = img.Url,
                            Width = (int)img.Width,
                            Height = (int)img.Height,
                            Formats = new FormatDto() 
                        }).ToList()
                        : new List<ImageDto>(),

                //Thumbnail = MapImage(p.ImagenProducto.ToList(), "front"),
                //ThumbnailBack = MapImage(p.ImagenProducto.ToList(), "back"),
                Thumbnail = MapImage(p.ImagenProducto?.ToList() ?? new List<ImagenProducto>(), "front"),
                ThumbnailBack = MapImage(p.ImagenProducto?.ToList() ?? new List<ImagenProducto>(), "back"),

                ProductCategories = p.Producto.Linea != null
                    ? new List<LineaDto>
                    {
                new LineaDto
                {
                    Id = p.Producto.Linea.Id,
                    Linea = p.Producto.Linea.Linea,
                    Slug = p.Producto.Linea.Slug
                }
                    }
                    : new List<LineaDto>(),

                ProductBrands = p.Producto.MarcaProducto != null
                    ? new List<MarcaProductoDto>
                    {
                new MarcaProductoDto
                {
                    Id = p.Producto.MarcaProducto.Id,
                    Marca = p.Producto.MarcaProducto.Marca,
                    Slug = p.Producto.MarcaProducto.Slug
                }
                    }
                    : new List<MarcaProductoDto>(),
                ProductosCaracteristicas = p.Producto.ProductosCaracteristicas != null
                                        ? p.Producto.ProductosCaracteristicas.Select(c => new ProductosCaracteristicasDTO
                                        {
                                            Id = c.Id,
                                            ProductoId = c.Productoid,
                                            Nombre = c.Nombre,
                                            Descripcion = c.Descripcion,
                                            FechaCreado = c.FechaCreado
                                        }).ToList()
                                        : new List<ProductosCaracteristicasDTO>()

            }).ToList();

            return productos;
        }
        

        public async Task<List<ProductoEcommerceDto>> GetProductoByIdAsync(int id)
        {
            var baseUrl = "";

            var defaultBadges = new List<MarcaProductoDto>
{
                new MarcaProductoDto { Id = 0, Marca = "NO DATO", Slug = "no-dato" }
            };

    

            var productoDto = await _context.ProductosEmpaque
            .Include(pe => pe.Producto)
                .ThenInclude(p => p.Linea)
            .Include(pe => pe.Producto)
                .ThenInclude(p => p.MarcaProducto)
            .Include(pe => pe.Empaque)
                .ThenInclude(e => e.UnidadSAT)
                 .Include(pe => pe.ImagenProducto)
                  .Include(pe => pe.Producto)
                  .ThenInclude(p => p.ProductosCaracteristicas)
     .Where(p => p.Id == id)
     .Select(p => new ProductoEcommerceDto
     {
         ProductoEmpaqueId = p.Id,
         Name =
        (p.Codigo ?? "NO DATO") + " - " +
        (p.Producto.NombreProducto ?? "NO DATO") + " - " +
        (
            p.Empaque != null ? p.Empaque.Empaque
            : "NO DATO"
        ),
         Featured = false,
         Price = p.PVenta ?? 0,
         SalePrice = null,
         OnSale = false,
         Slug = p.Producto.Slug ?? "no-dato",
         IsStock = true,
         RatingCount = p.Producto.Rating ?? 0,
         Description = p.Producto.Descripcion ?? "NO DATO",
         ShortDescription = p.Producto.DescripcionBreve ?? "NO DATO",
         CategoriaTipo = p.Producto.CategoriaTipo ?? "Default",
         CreatedAt = DateTime.Now,
         UpdatedAt = DateTime.Now,
         Sizes = _context.ProductosEmpaque
            .Where(pe2 => pe2.ProductoId == p.ProductoId)
            .Select(pe2 => new EmpaqueDto
            {
                Id = pe2.EmpaqueId,
                ProductoEmpaqueId = pe2.Id, // <-- Aquí incluyes el ID de ProductoEmpaque
                Empaque = pe2.Empaque.Empaque,
                Contenido = pe2.Empaque.Contenido,
                Sincronizado = pe2.Empaque.Sincronizado,
                CodigoEmpaque = pe2.Empaque.CodigoEmpaque,
                Codigo = pe2.Codigo ?? "NO DATO",
                PCompra = pe2.PCompra ?? 0,
                PVenta = pe2.PVenta ?? 0,
                Descuento = pe2.Descuento ?? 0,
                Activo = pe2.Activo ?? false,
                FechaCreado = pe2.Empaque.FechaCreado,
                UnidadSat = pe2.Empaque.UnidadSAT != null ? new UnidadSatDto
                {
                    Id = pe2.Empaque.UnidadSAT.Id,
                    ClaveUnidad = pe2.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                    UnidadSat = pe2.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
                } : null
            })
            .ToList(),
         Colors = new List<string> { "#eb7b8b", "#000000", "#927764" },
         Badges = p.Producto.MarcaProducto != null
             ? new List<MarcaProductoDto> {
                new MarcaProductoDto
                {
                    Id = p.Producto.MarcaProducto.Id,
                    Marca = p.Producto.MarcaProducto.Marca ?? "NO DATO",
                    Slug = p.Producto.MarcaProducto.Slug ?? "NO DATO"
                }
             }
             : defaultBadges,
        
         Images = p.ImagenProducto.Any() == true
                        ? p.ImagenProducto.Select(img => new ImageDto
                        {
                            Id = img.Id,
                            Name = System.IO.Path.GetFileName(img.Url),
                            Url = img.Url,
                            Width = (int)img.Width,
                            Height = (int)img.Height,
                            Formats = new FormatDto()
                        }).ToList()
                        : new List<ImageDto>(),
         Thumbnail = MapImage(p.ImagenProducto.ToList() ?? new List<ImagenProducto>(), "front"),
         ThumbnailBack = MapImage(p.ImagenProducto.ToList() ?? new List<ImagenProducto>(), "back"),

         ProductCategories = p.Producto.Linea != null? new List<LineaDto> {
                            new LineaDto { Id = p.Producto.Linea.Id, Linea = p.Producto.Linea.Linea, Slug = p.Producto.Linea.Slug }
                         }
                         : new List<LineaDto>(),
         ProductBrands = p.Producto.MarcaProducto != null? new List<MarcaProductoDto> {
                            new MarcaProductoDto { Id = p.Producto.MarcaProducto.Id, Marca = p.Producto.MarcaProducto.Marca, Slug = p.Producto.MarcaProducto.Slug }
                         }
                         : new List<MarcaProductoDto>(),
         ProductosCaracteristicas = p.Producto.ProductosCaracteristicas != null
                                        ? p.Producto.ProductosCaracteristicas.Select(c => new ProductosCaracteristicasDTO
                                        {
                                            Id = c.Id,
                                            ProductoId = c.Productoid,
                                            Nombre = c.Nombre,
                                            Descripcion = c.Descripcion,
                                            FechaCreado = c.FechaCreado
                                        }).ToList()
                                        : new List<ProductosCaracteristicasDTO>()
     })
                 .ToListAsync();

          

            return   productoDto;
  
        }




        public async Task<List<ProductoEcommerceDto>> GetProductosLimitAsync(int limit = 10, int offset = 0)
        {
            var baseUrl ="";

            var defaultBadges = new List<MarcaProductoDto>
            {
                new MarcaProductoDto { Id = 0, Marca = "NO DATO", Slug = "no-dato" }
            };



            var productosRaw = await _context.ProductosEmpaque
            .Include(pe => pe.Producto)
            
                .ThenInclude(p => p.Linea)
            .Include(pe => pe.Producto)
               .ThenInclude(p => p.MarcaProducto)
            .Include(pe => pe.Empaque)
               .ThenInclude(e => e.UnidadSAT)
                 .Include(pe => pe.ImagenProducto)
                  .Include(pe => pe.Producto)
                  .ThenInclude(p => p.ProductosCaracteristicas)
                  .Skip(offset)
      .Take(limit)
                 //.Where(p => p.ProductoId == id)
                 //                   .Where(p => p.Id == 1254)
                 .ToListAsync();

            // SEGUNDA PARTE: Creamos los DTOs en memoria
            var productos = productosRaw.Select(p => new ProductoEcommerceDto
            {
                ProductoEmpaqueId = p.Id,
                Name = $"{p?.Codigo ?? "SIN-COD"} - {p.Producto.NombreProducto ?? "NO DATO"} - {p?.Empaque?.Empaque ?? "SIN UNIDAD"}",
                Featured = false,
                Price = p?.PVenta ?? 0,
                SalePrice = null,
                OnSale = false,
                Slug = p.Producto.Slug ?? "no-dato",
                IsStock = true,
                RatingCount = p.Producto.Rating ?? 0,
                Description = p.Producto.Descripcion ?? "NO DATO",
                ShortDescription = p.Producto.DescripcionBreve ?? "NO DATO",
                CategoriaTipo = p.Producto.CategoriaTipo ?? "Default",
                CreatedAt = p.Producto.CreatedAt,
                UpdatedAt = p.Producto.UpdatedAt,

                Sizes = new List<EmpaqueDto>
                    {
                        new EmpaqueDto
                        {
                            Id = p.EmpaqueId,
                            Empaque = p.Empaque.Empaque,
                            Contenido = p.Empaque.  Contenido,
                            Sincronizado = p.Empaque.Sincronizado,
                            CodigoEmpaque = p.Empaque.CodigoEmpaque,
                            Codigo = p.Codigo ?? "NO DATO",
                            PCompra = p.PCompra ?? 0,
                            PVenta = p.PVenta ?? 0,
                            Descuento = p.Descuento ?? 0,
                            Activo = p.Activo ?? false,
                            FechaCreado = p.Empaque.FechaCreado,
                            UnidadSat = p.Empaque?.UnidadSAT != null ? new UnidadSatDto
                            {
                                Id = p.Empaque.UnidadSAT.Id,
                                ClaveUnidad = p.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                                UnidadSat = p.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
                            } : null
                        }
                    },

                Colors = new List<string> { "#eb7b8b", "#000000", "#927764" }, // Estático por ahora

                Badges = p.Producto.MarcaProducto != null
                    ? new List<MarcaProductoDto>
                    {
                new MarcaProductoDto
                {
                    Id = p.Producto.MarcaProducto.Id,
                    Marca = p.Producto.MarcaProducto.Marca ?? "NO DATO",
                    Slug = p.Producto.MarcaProducto.Slug ?? "NO DATO"
                }
                    }
                    : defaultBadges,
                Images = p.ImagenProducto != null && p.ImagenProducto.Any()
                    ? p.ImagenProducto.Select(img => new ImageDto
                    {
                        Id = img.Id,
                        Name = System.IO.Path.GetFileName(img.Url),
                        Url = img.Url,
                        Width = (int)img.Width,
                        Height = (int)img.Height,
        
                        Formats = new FormatDto()
                    }).ToList()
                    : new List<ImageDto>(),
                Thumbnail = MapImage(p.ImagenProducto?.ToList() ?? new List<ImagenProducto>(), "front"),
                ThumbnailBack = MapImage(p.ImagenProducto?.ToList() ?? new List<ImagenProducto>(), "back"),


                ProductCategories = p.Producto.Linea != null
                    ? new List<LineaDto>
                    {
                new LineaDto
                {
                    Id = p.Producto.Linea.Id,
                    Linea = p.Producto.Linea.Linea,
                    Slug = p.Producto.Linea.Slug
                }
                    }
                    : new List<LineaDto>(),

                ProductBrands = p.Producto.MarcaProducto != null
                    ? new List<MarcaProductoDto>
                    {
                new MarcaProductoDto
                {
                    Id = p.Producto.MarcaProducto.Id,
                    Marca = p.Producto.MarcaProducto.Marca,
                    Slug = p.Producto.MarcaProducto.Slug
                }
                    }
                    : new List<MarcaProductoDto>(),
                ProductosCaracteristicas = p.Producto.ProductosCaracteristicas != null
                                        ? p.Producto.ProductosCaracteristicas.Select(c => new ProductosCaracteristicasDTO
                                        {
                                            Id = c.Id,
                                            ProductoId = c.Productoid,
                                            Nombre = c.Nombre,
                                            Descripcion = c.Descripcion,
                                            FechaCreado = c.FechaCreado
                                        }).ToList()
                                        : new List<ProductosCaracteristicasDTO>()

            }).ToList();

            return productos;
        }

        public async Task<List<LineaDto>>GetProductosCategories(int categoriaId=0,string slug = "null")
        {

            var query = _context.Lineas
        .Include(l => l.Productos)
            .ThenInclude(p => p.MarcaProducto)
        .Include(l => l.Productos)
            .ThenInclude(p => p.ProductosEmpaque)
                .ThenInclude(pe => pe.ImagenProducto)
        .Include(l => l.Productos)
            .ThenInclude(p => p.ProductosEmpaque)
                .ThenInclude(pe => pe.Empaque)
        .Include(l => l.Productos)
            .ThenInclude(p => p.ProductosCaracteristicas)

            .AsQueryable();


            if (categoriaId != 0)
            {
                query = query.Where(ci => ci.Id == categoriaId);
            }

            if (!string.IsNullOrWhiteSpace(slug) && slug != "null")
            {
                query = query.Where(ci => ci.Slug.Contains(slug));
            }
            
            var items = await query.ToListAsync();

            var lineas = items.Select(linea => new LineaDto
            {
                Id = linea.Id,
                Linea = linea.Linea,
                Slug = linea.Slug,
                FechaCreado = linea.FechaCreado,
                ProductoEmpaque = linea.Productos
                         .SelectMany(p => p.ProductosEmpaque.Select(pe => new ProductoEmpaqueDto
                         {
                             ProductoEmpaqueId = pe.Id,
                             ProductoId = pe.ProductoId,
                             EmpaqueId = pe.EmpaqueId,
                             Codigo = pe.Codigo ?? "NO DATO",
                             PCompra = pe.PCompra ?? 0,
                             PVenta = pe.PVenta ?? 0,
                             Descuento = (float?)(pe.Descuento ?? 0),
                             Activo = pe.Activo ?? false,

                             Producto = new ProductoDto
                             {
                                 ProductId = p.Id,
                                 ProductoSatId = p.ProductoSatId,
                               
                                 Prefijo = p.Prefijo ?? "NO DATO",
                                 NombreProducto = $"{pe?.Codigo ?? "SIN-COD"} - {p.NombreProducto ?? "NO DATO"} - {pe?.Empaque?.Empaque ?? "SIN UNIDAD"}",
                                 Descripcion = p.Descripcion ?? "NO DATO",
                                 DescripcionBreve = p.DescripcionBreve ?? "NO DATO",
                                 Slug = p.Slug ?? "no-slug",
                                 Rating = p.Rating ?? 0,
                                 Acumulador = p.Acumulador ?? false,
                                 ProductoIdAcumulador = p.ProductoIdAcumulador ?? 0,
                                 CategoriaTipo = p.CategoriaTipo ?? "Default",
                                 ProductosCaracteristicas = p.ProductosCaracteristicas != null
                                        ? p.ProductosCaracteristicas.Select(c => new ProductosCaracteristicasDTO
                                        {
                                            Id = c.Id,
                                            ProductoId = c.Productoid,
                                            Nombre = c.Nombre,
                                            Descripcion = c.Descripcion,
                                            FechaCreado = c.FechaCreado
                                        }).ToList()
                                        : new List<ProductosCaracteristicasDTO>()
                             },

                             Empaque = pe.Empaque != null ? new EmpaqueDto
                             {
                                 Id = pe.Empaque.Id,
                                 Empaque = pe.Empaque.Empaque,
                                 Contenido = pe.Empaque.Contenido,
                                 Sincronizado = pe.Empaque.Sincronizado,
                                 CodigoEmpaque = pe.Empaque.CodigoEmpaque,
                                 Codigo = pe.Empaque.CodigoEmpaque ?? "NO DATO"
                             } : null,

                             ImagenProducto = pe.ImagenProducto != null && pe.ImagenProducto.Any()
                                ? pe.ImagenProducto
                                    .Select(img => new ImageDto
                                    {
                                        Id = img.Id,
                                        Name = System.IO.Path.GetFileName(img.Url),
                                        Url = img.Url,
                                        Width = (int)(img.Width ?? 800),
                                        Height = (int)(img.Height ?? 800),
                                        Formats = new FormatDto()
                                    }).ToList()
                                : new List<ImageDto>(),

                           
                             
                         }))
                         .ToList()
            }).ToList();

        return lineas;

        }

        //no me gusto que tarda mucho xd 
        public async Task<List<catLineaDTO>> GetProductsCategories(int categoryId = 0, string slug = "null")
        {

            string baseUrl = "";



            var resultadoPlano = await _context.Set<ProductoPlano>()
            .FromSqlInterpolated($"EXEC spCategorias_productosGet @categoria_id={categoryId}, @categoria_like={slug}")
        .ToListAsync();

            // Aquí haces el agrupamiento como te mostré antes
            var lineasDict = new Dictionary<int, catLineaDTO>();

            foreach (var row in resultadoPlano)
            {
                if (!lineasDict.TryGetValue(row.Id, out var linea))
                {
                    linea = new catLineaDTO
                    {
                        Id = row.Id,
                        Name = row.Name_Linea,
                        Slug = row.Slug_Linea.ToLower().Replace(" ", "-"),
                        Products = new List<catProductoDTO>()
                    };
                    lineasDict[row.Id] = linea;
                }

                var producto = linea.Products.FirstOrDefault(p => p.Id == row.Id_Producto);
                if (producto == null)
                {
                    producto = new catProductoDTO
                    {
                        Id = row.Id_Producto,
                        Name = row.NombreProducto,
                        Price = row.PVenta ?? 0,
                        Description = row.Descripcion,
                        OnSale = false,
                        IsStock = true,
                        Slug = row.SlugProducto.ToLower().Replace(" ", "-"),
                        Sizes = new List<cSizeDTO>(),
                        Images = new List<cImageDTO>()
                    };
                    linea.Products.Add(producto);
                }

                if (row.Id_Size.HasValue && !producto.Sizes.Any(s => s.Id == row.Id_Size))
                {
                    producto.Sizes.Add(new cSizeDTO
                    {
                        Id = row.Id_Size.Value,
                        Codigo = row.Codigo_Empaque,
                        PVenta = row.PVentaPE,
                        PCompra = row.PCompraPE,
                        Descuento = row.DescuentoPE,
                        Activo = row.ActivoPE
                    });
                }

                if (row.id_imagen.HasValue && !producto.Images.Any(i => i.Id == row.id_imagen))
                {
                    var imagen = new cImageDTO
                    {
                        Id = 0,
                        Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                        Name = $"img_{row.id_imagen}"
                    };

                    producto.Images.Add(imagen);

                    if (row.EsImagenPrincipal == true && producto.Thumbnail == null)
                        producto.Thumbnail = imagen;
                }
            }

            return lineasDict.Values.ToList();


        }
        //ya aya que quitarlo
        public async Task<List<ProductoEcommerceDto>> GetProductosNameContainsAsync(string? name_contains = null)
        {
            var baseUrl = "";

            var defaultBadges = new List<MarcaProductoDto>
    {
        new MarcaProductoDto { Id = 0, Marca = "NO DATO", Slug = "no-dato" }
    };

            var query = _context.ProductosEmpaque
     .Include(pe => pe.Producto)
         .ThenInclude(p => p.Linea)
     .Include(pe => pe.Producto)
         .ThenInclude(p => p.MarcaProducto)
     .Include(pe => pe.Producto)
         .ThenInclude(p => p.ProductosCaracteristicas)  
     .Include(pe => pe.Empaque)
         .ThenInclude(e => e.UnidadSAT)
     .Include(pe => pe.ImagenProducto)
     .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name_contains))
            {
                query = query.Where(pe =>
                    pe.Producto.NombreProducto.ToLower().Contains(name_contains.ToLower()));
            }

            var productosRaw = await query
               
                .ToListAsync();

            // SEGUNDA PARTE: Creamos los DTOs en memoria
            var productos = productosRaw.Select(p => new ProductoEcommerceDto
            {
                ProductoEmpaqueId = p.Id,
                Name = $"{p?.Codigo ?? "SIN-COD"} - {p.Producto.NombreProducto ?? "NO DATO"} - {p?.Empaque?.Empaque ?? "SIN UNIDAD"}",
                Featured = false,
                Price = p?.PVenta ?? 0,
                SalePrice = 0,
                OnSale = false,
                Slug = p.Producto.Slug ?? "no-dato",
                IsStock = true,
                RatingCount = p.Producto.Rating ?? 0,
                Description = p.Producto.Descripcion ?? "NO DATO",
                ShortDescription = p.Producto.DescripcionBreve ?? "NO DATO",
                CategoriaTipo = p.Producto.CategoriaTipo ?? "Default",
                CreatedAt = p.Producto.CreatedAt,
                UpdatedAt = p.Producto.UpdatedAt,

                Sizes = new List<EmpaqueDto>
                    {
                        new EmpaqueDto
                        {
                            Id = p.EmpaqueId,
                            Empaque = p.Empaque.Empaque,
                            Contenido = p.Empaque.  Contenido,
                            Sincronizado = p.Empaque.Sincronizado,
                            CodigoEmpaque = p.Empaque.CodigoEmpaque,
                            Codigo = p.Codigo ?? "NO DATO",
                            PCompra = p.PCompra ?? 0,
                            PVenta = p.PVenta ?? 0,
                            Descuento = p.Descuento ?? 0,
                            Activo = p.Activo ?? false,
                            FechaCreado = p.Empaque.FechaCreado,
                            UnidadSat = p.Empaque?.UnidadSAT != null ? new UnidadSatDto
                            {
                                Id = p.Empaque.UnidadSAT.Id,
                                ClaveUnidad = p.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                                UnidadSat = p.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
                            } : null
                        }
                    },

                Colors = new List<string> { "#eb7b8b", "#000000", "#927764" }, // Estático por ahora

                Badges = p.Producto.MarcaProducto != null
                    ? new List<MarcaProductoDto>
                    {
                new MarcaProductoDto
                {
                    Id = p.Producto.MarcaProducto.Id,
                    Marca = p.Producto.MarcaProducto.Marca ?? "NO DATO",
                    Slug = p.Producto.MarcaProducto.Slug ?? "NO DATO"
                }
                    }
                    : defaultBadges,

                Images =  p.ImagenProducto != null && p.ImagenProducto.Any()
                        ? p.ImagenProducto.Select(img => new ImageDto
                        { 
                            Id= img.Id,
                            Name = System.IO.Path.GetFileName(img.Url),
                            Url = img.Url,
                            Width = (int)img.Width,
                            Height = (int)img.Height,
                            Formats = new FormatDto()
                        }).ToList()
                        : new List<ImageDto>(),
                Thumbnail = MapImage(p.ImagenProducto?.ToList() ?? new List<ImagenProducto>(), "front"),
                ThumbnailBack = MapImage(p.ImagenProducto?.ToList() ?? new List<ImagenProducto>(), "back"),


                ProductCategories = p.Producto.Linea != null
                    ? new List<LineaDto>
                    {
                new LineaDto
                {
                    Id = p.Producto.Linea.Id,
                    Linea = p.Producto.Linea.Linea,
                    Slug = p.Producto.Linea.Slug
                }
                    }
                    : new List<LineaDto>(),

                ProductBrands = p.Producto.MarcaProducto != null
                    ? new List<MarcaProductoDto>
                    {
                new MarcaProductoDto
                {
                    Id = p.Producto.MarcaProducto.Id,
                    Marca = p.Producto.MarcaProducto.Marca,
                    Slug = p.Producto.MarcaProducto.Slug
                }
                    }
                    : new List<MarcaProductoDto>(),
                     ProductosCaracteristicas = p.Producto.ProductosCaracteristicas != null
                        ? p.Producto.ProductosCaracteristicas.Select(c => new ProductosCaracteristicasDTO
                        {
                            Id = c.Id,
                            ProductoId = c.Productoid,
                            Nombre = c.Nombre,
                            Descripcion = c.Descripcion,
                            FechaCreado = c.FechaCreado
                        }).ToList()
                        : new List<ProductosCaracteristicasDTO>()


            }).ToList();

            return productos;
        }

        public IEnumerable<ProductoEmpaqueDto> GetAllProductCategories()
        {
            return _context.ProductosEmpaque
                //.Where(c => c.Activo) // Opcional
                .Select(c => new ProductoEmpaqueDto
                {
                    ProductoEmpaqueId = c.Id,
                    Codigo = c.Codigo
                }).ToList();
        }

        private static ImageDto MapImage(List<ImagenProducto> imagenes, string label)
        {
            var baseUrl = ""; // Defínelo si usas dominio

            var grouped = imagenes
                .Where(img => img.Label == label)
                .GroupBy(img => img.Type)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());

            grouped.TryGetValue("original", out var original);
            grouped.TryGetValue("thumbnail", out var thumbnail);
            grouped.TryGetValue("small", out var small);
            grouped.TryGetValue("medium", out var medium);
            grouped.TryGetValue("large", out var large);

            return new ImageDto
            {
                Id = original?.Id ?? 0,
                Name = original != null ? System.IO.Path.GetFileName(original.Url) : "no-image.jpg",
                Url = original?.Url ?? $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                Width = (int)(original?.Width ?? 800),
                Height = (int)(original?.Height ?? 800),
                Formats = new FormatDto
                {
                    Thumbnail = thumbnail != null ? new FormatItemDto
                    {
                        Url = thumbnail.Url,
                        Width = (int)(thumbnail.Width),
                        Height = (int)(thumbnail.Height)
                    } : null,
                    Small = small != null ? new FormatItemDto
                    {
                        Url = small.Url,
                        Width = (int)(small.Width),
                        Height = (int)(small.Height)
                    } : null,
                    Medium = medium != null ? new FormatItemDto
                    {
                        Url = medium.Url,
                        Width = (int)(medium.Width),
                        Height = (int)(medium.Height)
                    } : null,
                    Large = large != null ? new FormatItemDto
                    {
                        Url = large.Url,
                        Width = (int)(large.Width),
                        Height = (int)(large.Height)
                    } : null,
                }
            };
        }
    }
}
