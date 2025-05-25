using Api_comerce.Data;
using Api_comerce.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace Api_comerce.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly AppDbContext _context;

        public ProductsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductoEcommerceDto>> GetAllProductosAsync()
        {

            var defaultBadges = new List<MarcaProductoDto>
{
                new MarcaProductoDto { Id = 0, Marca = "NO DATO", Slug = "no-dato" }
            };

            return await _context.Productos
                .Include(p => p.Linea)
                .Include(p => p.MarcaProducto)
                .Include(p => p.ProductoSat)
                .Include(p => p.ProductosEmpaque)
                    .ThenInclude(pe => pe.Empaque)
                        .ThenInclude(e => e.UnidadSAT)
                //.Include(p => p.Imagenes) //no tenemos imagenes c
                .Select(p => new ProductoEcommerceDto
                {
                    Id = p.Id,
                    Name = p.NombreProducto ?? "NO DATO",
                    Featured = false,
                    Price = p.ProductosEmpaque.FirstOrDefault().PVenta ?? 0,
                    SalePrice = null, 
                    OnSale = false, 
                    Slug = p.Slug ?? "no-dato",
                    IsStock = true, 
                    RatingCount = p.Rating ?? 0,
                    Description = p.Descripcion ?? "NO DATO",
                    ShortDescription = p.DescripcionBreve ?? "NO DATO",
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Sizes = p.ProductosEmpaque != null
                        ? p.ProductosEmpaque.Select(e => new EmpaqueDto
                        {
                            Id = e.EmpaqueId,
                            Codigo = e.Codigo ?? "NO DATO",
                            PCompra = e.PCompra ?? 0,
                            PVenta = e.PVenta ?? 0,
                            Descuento = e.Descuento ?? 0,
                            Activo = e.Activo ?? false,
                            UnidadSat = e.Empaque != null && e.Empaque.UnidadSAT != null ? new UnidadSatDto
                            {
                                Id = e.Empaque.UnidadSAT.Id,
                                ClaveUnidad = e.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                                UnidadSat = e.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
                            } : null
                        }).ToList()
                        : new List<EmpaqueDto>(),
                    Colors = new List<string> { "#eb7b8b", "#000000", "#927764" }, // checar en la bd de que tabla traer
                    Badges = p.MarcaProducto != null
                 ? new List<MarcaProductoDto>
                   { new MarcaProductoDto
                     {
                       Id = p.MarcaProducto.Id,
                       Marca = p.MarcaProducto.Marca ?? "NO DATO",
                       Slug = p.MarcaProducto.Slug ?? "NO DATO"
                     }
                   }
                 : defaultBadges,
                    Images = new List<ImageDto>
                    {
                        new ImageDto
                        {
                            Id = 0,
                            Name = "NO DATO",
                            Width = 0,
                            Height = 0,
                            Url = "NO DATO",
                            Formats = new FormatDto
                            {
                                Thumbnail = new FormatItemDto { Url = "NO DATO", Width = 156, Height = 156 },
                                Small = new FormatItemDto { Url = "NO DATO", Width = 500, Height = 500 },
                                Medium = new FormatItemDto { Url = "NO DATO", Width = 750, Height = 750 },
                                Large = new FormatItemDto { Url = "NO DATO", Width = 1000, Height = 1000 }
                            }
                        }
                    },

                    //Images = p.Imagenes != null && p.Imagenes.Any()
                    //? p.Imagenes.Select(img => new ImagenDto
                    //{
                    //    Id = img.Id,
                    //    Name = img.Nombre ?? "NO DATO",
                    //    Width = img.Ancho,
                    //    Height = img.Alto,
                    //    Url = img.Url ?? "NO DATO",
                    //    Formats = new FormatDto
                    //    {
                    //        Thumbnail = new FormatItemDto { Url = img.ThumbnailUrl ?? "NO DATO", Width = 156, Height = 156 },
                    //        Small = new FormatItemDto { Url = img.SmallUrl ?? "NO DATO", Width = 500, Height = 500 },
                    //        Medium = new FormatItemDto { Url = img.MediumUrl ?? "NO DATO", Width = 750, Height = 750 },
                    //        Large = new FormatItemDto { Url = img.LargeUrl ?? "NO DATO", Width = 1000, Height = 1000 }
                    //    }
                    //}).ToList()
                    //: new List<ImagenDto>
                    //{
                    //    new ImagenDto
                    //    {
                    //        Id = 0,
                    //        Name = "NO DATO",
                    //        Width = 0,
                    //        Height = 0,
                    //        Url = "NO DATO",
                    //        Formats = new FormatDto
                    //        {
                    //            Thumbnail = new FormatItemDto { Url = "NO DATO", Width = 156, Height = 156 },
                    //            Small = new FormatItemDto { Url = "NO DATO", Width = 500, Height = 500 },
                    //            Medium = new FormatItemDto { Url = "NO DATO", Width = 750, Height = 750 },
                    //            Large = new FormatItemDto { Url = "NO DATO", Width = 1000, Height = 1000 }
                    //        }
                    //    }
                    //},

                    Thumbnail =  new ImageDto
                    {
                        Id = 0,
                        Name = "NO DATO",
                        Width = 100,
                        Height = 80,
                        Url = "http://url",
                        Formats = new FormatDto
                        {
                            Thumbnail = new FormatItemDto { Url = "http://urlNODATO", Width = 156, Height = 156 },
                            Small = new FormatItemDto { Url = "http://urlNODATO", Width = 500, Height = 500 },
                            Medium = new FormatItemDto { Url = "http://urlNODATO", Width = 750, Height = 750 },
                            Large = new FormatItemDto { Url = "http://urlNODATO", Width = 1000, Height = 1000 }
                        }
                    } ,

                    ThumbnailBack = new ImageDto
                    {
                        Id = 1,
                        Name = "NO DATO",
                        Width = 400,
                        Height = 270,
                        Url = "http://urlNODATO",
                        Formats = new FormatDto
                        {
                            Thumbnail = new FormatItemDto { Url = "http://urlNODATO", Width = 156, Height = 156 },
                            Small = new FormatItemDto { Url = "http://urlNODATO", Width = 500, Height = 500 },
                            Medium = new FormatItemDto { Url = "http://urlNODATO", Width = 750, Height = 750 },
                            Large = new FormatItemDto { Url = "http://urlNODATO", Width = 1000, Height = 1000 }
                        }
                    } ,

                    ProductCategories = p.Linea != null ? new List<LineaDto>
                    {
                        new LineaDto
                        {
                            Id = p.Linea.Id,
                             Linea = p.Linea.Linea,
                            Slug = p.Linea.Slug
                        }
                    } : new List<LineaDto>(),

                    ProductBrands = p.MarcaProducto != null ? new List<MarcaProductoDto>
                            {
                                new MarcaProductoDto
                                {
                                    Id = p.MarcaProducto.Id,
                                    Marca = p.MarcaProducto.Marca,
                                    Slug = p.MarcaProducto.Slug
                                }
                    } : new List<MarcaProductoDto>()
                 })
                .ToListAsync();
        }

        public async Task<ProductoEcommerceDto?>GetProductoByIdAsync(int id)
        {


            var defaultBadges = new List<MarcaProductoDto>
{
                new MarcaProductoDto { Id = 0, Marca = "NO DATO", Slug = "no-dato" }
            };

            var productoDto = await _context.Productos
     .Include(p => p.Linea)
     .Include(p => p.MarcaProducto)
     .Include(p => p.ProductoSat)
     .Include(p => p.ProductosEmpaque)
         .ThenInclude(pe => pe.Empaque)
             .ThenInclude(e => e.UnidadSAT)
     .Where(p => p.Id == id)
     .Select(p => new ProductoEcommerceDto
     {
         Id = p.Id,
         Name = p.NombreProducto ?? "NO DATO",
         Featured = false,
         Price = p.ProductosEmpaque.FirstOrDefault().PVenta ?? 0,
         SalePrice = null,
         OnSale = false,
         Slug = p.Slug ?? "no-dato",
         IsStock = true,
         RatingCount = p.Rating ?? 0,
         Description = p.Descripcion ?? "NO DATO",
         ShortDescription = p.DescripcionBreve ?? "NO DATO",
         CreatedAt = DateTime.Now,
         UpdatedAt = DateTime.Now,
         Sizes = p.ProductosEmpaque != null
             ? p.ProductosEmpaque.Select(e => new EmpaqueDto
             {
                 Id = e.EmpaqueId,
                 Codigo = e.Codigo ?? "NO DATO",
                 PCompra = e.PCompra ?? 0,
                 PVenta = e.PVenta ?? 0,
                 Descuento = e.Descuento ?? 0,
                 Activo = e.Activo ?? false,
                 UnidadSat = e.Empaque != null && e.Empaque.UnidadSAT != null ? new UnidadSatDto
                 {
                     Id = e.Empaque.UnidadSAT.Id,
                     ClaveUnidad = e.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                     UnidadSat = e.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
                 } : null
             }).ToList()
             : new List<EmpaqueDto>(),
         Colors = new List<string> { "#eb7b8b", "#000000", "#927764" },
         Badges = p.MarcaProducto != null
             ? new List<MarcaProductoDto> {
                new MarcaProductoDto
                {
                    Id = p.MarcaProducto.Id,
                    Marca = p.MarcaProducto.Marca ?? "NO DATO",
                    Slug = p.MarcaProducto.Slug ?? "NO DATO"
                }
             }
             : defaultBadges,
         // Aquí NO asignar Images todavía
         Thumbnail = new ImageDto
         {
             Id = 0,
             Name = "NO DATO",
             Width = 100,
             Height = 80,
             Url = "http://url",
             Formats = new FormatDto
             {
                 Thumbnail = new FormatItemDto { Url = "http://urlNODATO", Width = 156, Height = 156 },
                 Small = new FormatItemDto { Url = "http://urlNODATO", Width = 500, Height = 500 },
                 Medium = new FormatItemDto { Url = "http://urlNODATO", Width = 750, Height = 750 },
                 Large = new FormatItemDto { Url = "http://urlNODATO", Width = 1000, Height = 1000 }
             }
         },
         ThumbnailBack = new ImageDto
         {
             Id = 1,
             Name = "NO DATO",
             Width = 400,
             Height = 270,
             Url = "http://urlNODATO",
             Formats = new FormatDto
             {
                 Thumbnail = new FormatItemDto { Url = "http://urlNODATO", Width = 156, Height = 156 },
                 Small = new FormatItemDto { Url = "http://urlNODATO", Width = 500, Height = 500 },
                 Medium = new FormatItemDto { Url = "http://urlNODATO", Width = 750, Height = 750 },
                 Large = new FormatItemDto { Url = "http://urlNODATO", Width = 1000, Height = 1000 }
             }
         },
         ProductCategories = p.Linea != null
             ? new List<LineaDto> {
                new LineaDto { Id = p.Linea.Id, Linea = p.Linea.Linea, Slug = p.Linea.Slug }
             }
             : new List<LineaDto>(),
         ProductBrands = p.MarcaProducto != null
             ? new List<MarcaProductoDto> {
                new MarcaProductoDto { Id = p.MarcaProducto.Id, Marca = p.MarcaProducto.Marca, Slug = p.MarcaProducto.Slug }
             }
             : new List<MarcaProductoDto>()
     })
     .FirstOrDefaultAsync();

            // Ahora en memoria asignamos Images, que EF no puede traducir dentro del Select
            if (productoDto != null)
            {
                productoDto.Images = new List<ImageDto>
    {
        new ImageDto
        {
            Id = 0,
            Name = "NO DATO",
            Width = 0,
            Height = 0,
            Url = "NO DATO",
            Formats = new FormatDto
            {
                Thumbnail = new FormatItemDto { Url = "NO DATO", Width = 156, Height = 156 },
                Small = new FormatItemDto { Url = "NO DATO", Width = 500, Height = 500 },
                Medium = new FormatItemDto { Url = "NO DATO", Width = 750, Height = 750 },
                Large = new FormatItemDto { Url = "NO DATO", Width = 1000, Height = 1000 }
            }
        }
    };
            }

            return productoDto;




           
        }
    }
}
