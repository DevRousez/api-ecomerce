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
     .ToListAsync();
              

            // SEGUNDA PARTE: Creamos los DTOs en memoria
            var productos = productosRaw.Select(p => new ProductoEcommerceDto
            {
                Id = p.Id,
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

                Images = p.ImagenProducto?.Any() == true
                        ? p.ImagenProducto.Select(img => new ImageDto
                        {
                            Name = System.IO.Path.GetFileName(img.Url),
                            Url = img.Url,
                            Width = (int)img.Width,
                            Height = (int)img.Height,
                            Formats = new FormatDto() 
                        }).ToList()
                        : new List<ImageDto>(),

                Thumbnail = MapImage(p.ImagenProducto.ToList(), "front"),
                ThumbnailBack = MapImage(p.ImagenProducto.ToList(), "back"),

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
                 .Include(pe => pe.ImagenProducto)
     .Where(p => p.Id == id)
     .Select(p => new ProductoEcommerceDto
     {
         Id = p.Id,
         Name =
        (p.Codigo ?? "NO DATO") + " - " +
        (p.Producto.NombreProducto ?? "NO DATO") + " - " +
        (
            p.Empaque != null &&
            p.Empaque.UnidadSAT != null
            ? p.Empaque.Empaque
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
           Images = p.ImagenProducto.Any() == true
                        ? p.ImagenProducto.Select(img => new ImageDto
                        {
                            Name = System.IO.Path.GetFileName(img.Url),
                            Url = img.Url,
                            Width = (int)img.Width,
                            Height = (int)img.Height,
                            Formats = new FormatDto()
                        }).ToList()
                        : new List<ImageDto>(),
         Thumbnail = MapImage(p.ImagenProducto.ToList(), "front"),
         ThumbnailBack = MapImage(p.ImagenProducto.ToList(), "back"),

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

            //foreach (var producto in productoDto)
            //{
            //    producto.Images = new List<ImageDto>
            //            {
            //                new ImageDto
            //                {
            //                    Id = 0,
            //                    Name = "Sayer-Generic.jpg",
            //                    Width = 800,
            //                    Height = 800,
            //                    Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
            //                    Formats = new FormatDto
            //                    {
            //                        Thumbnail = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 156, Height = 156 },
            //                        Small = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 500, Height = 500 },
            //                        Medium = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 750, Height = 750 },
            //                        Large = new FormatItemDto { Url = $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg", Width = 1000, Height = 1000 }
            //                    }
            //                }
            //             };
            //}

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
                Images = p.ImagenProducto.Any() == true
                        ? p.ImagenProducto.Select(img => new ImageDto
                        {
                            Name = System.IO.Path.GetFileName(img.Url),
                            Url = img.Url,
                            Width = (int)img.Width,
                            Height = (int)img.Height,
                            Formats = new FormatDto()
                        }).ToList()
                        : new List<ImageDto>(),
                Thumbnail = MapImage(p.ImagenProducto.ToList(), "front"),
                ThumbnailBack = MapImage(p.ImagenProducto.ToList(), "back"),


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

        public async Task<List<LineaDto>>GetProductosCategories(int categoriaId=0,string slug = "null")
        {

            var query =  _context.Lineas
    .Include(ci => ci.Productos)
        .ThenInclude(p => p.MarcaProducto)
    .Include(ci => ci.Productos)
        .ThenInclude(p => p.ProductosEmpaque)
        .AsQueryable();
           

            if (categoriaId != 0)
            {
                query = query.Where(ci => ci.Id == categoriaId);
            }

            if (!string.IsNullOrWhiteSpace(slug) && slug != "null")
            {
                query = query.Where(ci => ci.Slug.Contains(slug));
            }

            var lineas = await query.ToListAsync();

            var items = lineas.Select(ci => new LineaDto
            {
                Id = ci.Id,
                Linea = ci.Linea,
                Slug = ci.Slug,
                FechaCreado = ci.FechaCreado,
                Productos = ci.Productos.Select(p => new ProductoDto
                {
                    Id = p.Id,
                    ProductoSatId = p.ProductoSatId,
                    Prefijo = p.Prefijo,
                    NombreProducto = p.NombreProducto,
                    Descripcion = p.Descripcion,
                    DescripcionBreve = p.DescripcionBreve,
                    Slug = p.Slug,
                    Rating = p.Rating,
                    Acumulador = p.Acumulador,
                    ProductoIdAcumulador = p.ProductoIdAcumulador,
                    Linea = new LineaDto
                    {
                        Id = ci.Id,
                        Linea = ci.Linea,
                        Slug = ci.Slug,
                        FechaCreado = ci.FechaCreado,
                    },
                    MarcaProducto = p.MarcaProducto == null ? new MarcaProductoDto
                    {
                        Id = 0,
                        Marca = "NO DATO",
                        Slug = "no-dato"
                    } : new MarcaProductoDto
                    {
                        Id = p.MarcaProducto.Id,
                        Marca = p.MarcaProducto.Marca,
                        Slug = p.MarcaProducto.Slug,
                    },
                    ProductoSat = new ProductoSatDto { },
                    UnidadSat = new UnidadSatDto { },
                    DescripcionProdSat = "",
                    productoEmpaques = p.ProductosEmpaque?.Select(pe => new ProductoEmpaqueDto
                    {
                        Id = pe.Id,
                        ProductoId = pe.ProductoId,
                        EmpaqueId = pe.EmpaqueId,
                        Codigo = pe.Codigo,
                        PCompra = pe.PCompra,
                        PVenta = pe.PVenta,
                        Descuento = (float?)pe.Descuento,
                        Activo = pe.Activo,
                    }).ToList() ?? new List<ProductoEmpaqueDto>()
                }).ToList()
            }).ToList();
            return items;

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
                .Include(pe => pe.Empaque)
                    .ThenInclude(e => e.UnidadSAT)
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

                Images = p.ImagenProducto.Any() == true
                        ? p.ImagenProducto.Select(img => new ImageDto
                        {
                            Name = System.IO.Path.GetFileName(img.Url),
                            Url = img.Url,
                            Width = (int)img.Width,
                            Height = (int)img.Height,
                            Formats = new FormatDto()
                        }).ToList()
                        : new List<ImageDto>(),
                Thumbnail = MapImage(p.ImagenProducto.ToList(), "front"),
                ThumbnailBack = MapImage(p.ImagenProducto.ToList(), "back"),


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

        public IEnumerable<ProductoEmpaqueDto> GetAllProductCategories()
        {
            return _context.ProductosEmpaque
                //.Where(c => c.Activo) // Opcional
                .Select(c => new ProductoEmpaqueDto
                {
                    Id = c.Id,
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

            return new ImageDto
            {
                Id = grouped["original"].Id,
                Name = grouped.ContainsKey("original") ? System.IO.Path.GetFileName(grouped["original"].Url) : "no-image.jpg",
                Url = grouped.ContainsKey("original") ? grouped["original"].Url : $"{baseUrl}/Assets/imagenes/products/Sayer-Generic.jpg",
                Width = (int)(grouped.ContainsKey("original") ? grouped["original"].Width : 800),
                Height = (int)(grouped.ContainsKey("original") ? grouped["original"].Height : 800),
                Formats = new FormatDto
                {
                    Thumbnail = grouped.ContainsKey("thumbnail") ? new FormatItemDto
                    {

                        Url = grouped["thumbnail"].Url,
                        Width = (int)(grouped["thumbnail"].Width),
                        Height = (int)(grouped["thumbnail"].Height)
                    } : null,
                    Small = grouped.ContainsKey("small") ? new FormatItemDto
                    {
                        Url = grouped["small"].Url,
                        Width = (int)(grouped["small"].Width),
                        Height = (int)(grouped["small"].Height)
                    } : null,
                    Medium = grouped.ContainsKey("medium") ? new FormatItemDto
                    {
                        Url = grouped["medium"].Url,
                        Width = (int)(grouped["medium"].Width),
                        Height = (int)(grouped["medium"].Height)
                    } : null,
                    Large = grouped.ContainsKey("large") ? new FormatItemDto
                    {
                        Url = grouped["large"].Url,
                        Width = (int)(grouped["large"].Width),
                        Height = (int)(grouped["large"].Height)
                    } : null,
                }
            };
        }
    }
}
