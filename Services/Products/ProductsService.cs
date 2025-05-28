using Api_comerce.Data;
using Api_comerce.Dtos;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Api_comerce.Models;


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
    //.Where(p => p.ProductoId == id)
    //                   .Where(p => p.Id == 1254)
                .ToListAsync();

            // SEGUNDA PARTE: Creamos los DTOs en memoria
            var productos = productosRaw.Select(p => new ProductoEcommerceDto
            {
                Id = p.Id,
                Name = $"{p?.Codigo ?? "SIN-COD"} - {p.Producto.NombreProducto ?? "NO DATO"} - {p?.Empaque?.UnidadSAT?.UnidadSat ?? "SIN UNIDAD"}",
                Featured = false,
                Price = p?.PVenta ?? 0,
                SalePrice = null,
                OnSale = false,
                Slug = p.Producto.Slug ?? "no-dato",
                IsStock = true,
                RatingCount = p.Producto.Rating ?? 0,
                Description = p.Producto.Descripcion ?? "NO DATO",
                ShortDescription = p.Producto.DescripcionBreve ?? "NO DATO",
                CreatedAt = p.Producto.CreatedAt,
                UpdatedAt = p.Producto.UpdatedAt,

                Sizes = p.Producto.ProductosEmpaque != null
                    ? p.Producto.ProductosEmpaque.Select(e => new EmpaqueDto
                    {
                        Id = e.EmpaqueId,
                        Codigo = e.Codigo ?? "NO DATO",
                        PCompra = e.PCompra ?? 0,
                        PVenta = e.PVenta ?? 0,
                        Descuento = e.Descuento ?? 0,
                        Activo = e.Activo ?? false,
                        UnidadSat = e.Empaque?.UnidadSAT != null ? new UnidadSatDto
                        {
                            Id = e.Empaque.UnidadSAT.Id,
                            ClaveUnidad = e.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                            UnidadSat = e.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
                        } : null
                    }).ToList()
                    : new List<EmpaqueDto>(),

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

                Images = new List<ImageDto>
        {
            new ImageDto
            {
                Id = 0,
                Name = "Sayer-Generic.jpg",
                Width = 800,
                Height = 800,
                Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                Formats = new FormatDto
                {
                    Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
                    Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
                    Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
                    Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
                }
            }
        },

                Thumbnail = new ImageDto
                {
                    Id = 0,
                    Name = "Sayer-Generic.jpg",
                    Width = 100,
                    Height = 80,
                    Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                    Formats = new FormatDto
                    {
                        Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
                        Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
                        Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
                        Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
                    }
                },

                ThumbnailBack = new ImageDto
                {
                    Id = 1,
                    Name = "Sayer-Generic.jpg",
                    Width = 400,
                    Height = 270,
                    Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                    Formats = new FormatDto
                    {
                        Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
                        Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
                        Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
                        Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
                    }
                },

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
                    : new List<MarcaProductoDto>()

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
     .Where(p => p.ProductoId == id)
     .Select(p => new ProductoEcommerceDto
     {
         Id = p.Id,
         Name =
        (p.Codigo ?? "NO DATO") + " - " +
        (p.Producto.NombreProducto ?? "NO DATO") + " - " +
        (
            p.Empaque != null &&
            p.Empaque.UnidadSAT != null
            ? p.Empaque.UnidadSAT.UnidadSat
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
         CreatedAt = DateTime.Now,
         UpdatedAt = DateTime.Now,
         Sizes = p.Producto.ProductosEmpaque != null
    ? p.Producto.ProductosEmpaque.Select(e => new EmpaqueDto
    {
        Id = e.EmpaqueId,
      
        Empaque = e.Empaque != null ? e.Empaque.Empaque ?? "NO DATO" : "NO DATO",
        Contenido = e.Empaque != null ? e.Empaque.Contenido ?? 0 : 0,
        Sincronizado = e.Empaque != null ? e.Empaque.Sincronizado ?? false : false,
        CodigoEmpaque = e.Empaque != null ? e.Empaque.CodigoEmpaque ?? "NO DATO" : "NO DATO",
        FechaCreado = e.Empaque != null ? e.Empaque.FechaCreado : null, // Mantenemos null para DateTime?
        Codigo = e.Codigo ?? "NO DATO",
        PCompra = e.PCompra ?? 0,
        PVenta = e.PVenta ?? 0,
        Descuento = e.Descuento ?? 0,
        Activo = e.Activo ?? false,
        UnidadSat = e.Empaque != null && e.Empaque.UnidadSAT != null
            ? new UnidadSatDto
            {
                Id = e.Empaque.UnidadSAT.Id,
                ClaveUnidad = e.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                UnidadSat = e.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
            }
            : null
    }).ToList()
    : new List<EmpaqueDto>(),
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
         // Aquí NO asignar Images todavía
         Thumbnail = new ImageDto
         {
             Id = 0,
             Name = "Sayer-Generic.jpg",
             Width = 100,
             Height = 80,
             Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
             Formats = new FormatDto
             {
                 Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
                 Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
                 Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
                 Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
             }
         },

         ThumbnailBack = new ImageDto
         {
             Id = 1,
             Name = "Sayer-Generic.jpg",
             Width = 400,
             Height = 270,
             Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
             Formats = new FormatDto
             {
                 Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
                 Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
                 Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
                 Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
             }
         },

         ProductCategories = p.Producto.Linea != null
             ? new List<LineaDto> {
                new LineaDto { Id = p.Producto.Linea.Id, Linea = p.Producto.Linea.Linea, Slug = p.Producto.Linea.Slug }
             }
             : new List<LineaDto>(),
         ProductBrands = p.Producto.MarcaProducto != null
             ? new List<MarcaProductoDto> {
                new MarcaProductoDto { Id = p.Producto.MarcaProducto.Id, Marca = p.Producto.MarcaProducto.Marca, Slug = p.Producto.MarcaProducto.Slug }
             }
             : new List<MarcaProductoDto>()
     })
     .ToListAsync();

            foreach (var producto in productoDto)
            {
                producto.Images = new List<ImageDto>
                        {
                            new ImageDto
                            {
                                Id = 0,
                                Name = "Sayer-Generic.jpg",
                                Width = 800,
                                Height = 800,
                                Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                                Formats = new FormatDto
                                {
                                    Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
                                    Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
                                    Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
                                    Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
                                }
                            }
                         };
            }

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
                  .Skip(offset)
      .Take(limit)
                 //.Where(p => p.ProductoId == id)
                 //                   .Where(p => p.Id == 1254)
                 .ToListAsync();

            // SEGUNDA PARTE: Creamos los DTOs en memoria
            var productos = productosRaw.Select(p => new ProductoEcommerceDto
            {
                Id = p.Id,
                Name = $"{p?.Codigo ?? "SIN-COD"} - {p.Producto.NombreProducto ?? "NO DATO"} - {p?.Empaque?.UnidadSAT?.UnidadSat ?? "SIN UNIDAD"}",
                Featured = false,
                Price = p?.PVenta ?? 0,
                SalePrice = null,
                OnSale = false,
                Slug = p.Producto.Slug ?? "no-dato",
                IsStock = true,
                RatingCount = p.Producto.Rating ?? 0,
                Description = p.Producto.Descripcion ?? "NO DATO",
                ShortDescription = p.Producto.DescripcionBreve ?? "NO DATO",
                CreatedAt = p.Producto.CreatedAt,
                UpdatedAt = p.Producto.UpdatedAt,

                Sizes = p.Producto.ProductosEmpaque != null
                    ? p.Producto.ProductosEmpaque.Select(e => new EmpaqueDto
                    {
                        Id = e.EmpaqueId,
                        Codigo = e.Codigo ?? "NO DATO",
                        PCompra = e.PCompra ?? 0,
                        PVenta = e.PVenta ?? 0,
                        Descuento = e.Descuento ?? 0,
                        Activo = e.Activo ?? false,
                        UnidadSat = e.Empaque?.UnidadSAT != null ? new UnidadSatDto
                        {
                            Id = e.Empaque.UnidadSAT.Id,
                            ClaveUnidad = e.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                            UnidadSat = e.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
                        } : null
                    }).ToList()
                    : new List<EmpaqueDto>(),

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

                Images = new List<ImageDto>
        {
            new ImageDto
            {
                Id = 0,
                Name = "Sayer-Generic.jpg",
                Width = 800,
                Height = 800,
                Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                Formats = new FormatDto
                {
                    Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
                    Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
                    Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
                    Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
                }
            }
        },

                Thumbnail = new ImageDto
                {
                    Id = 0,
                    Name = "Sayer-Generic.jpg",
                    Width = 100,
                    Height = 80,
                    Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                    Formats = new FormatDto
                    {
                        Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
                        Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
                        Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
                        Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
                    }
                },

                ThumbnailBack = new ImageDto
                {
                    Id = 1,
                    Name = "Sayer-Generic.jpg",
                    Width = 400,
                    Height = 270,
                    Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                    Formats = new FormatDto
                    {
                        Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
                        Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
                        Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
                        Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
                    }
                },

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
                    : new List<MarcaProductoDto>()

            }).ToList();

            return productos;
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            if (request == null)
                return string.Empty;

            return $"{request.Scheme}://{request.Host}";
        }
    }
}
