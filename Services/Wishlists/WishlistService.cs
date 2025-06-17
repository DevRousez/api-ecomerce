using Api_comerce.Models;
using Google;
using Api_comerce.Data;
using Microsoft.EntityFrameworkCore;
using Api_comerce.Dtos;

namespace Api_comerce.Services.Wishlists
{
    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _context;

        public WishlistService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WishlistResponseDto?> GetWishlistByUserAsync(int userId)
        {
            var wishlist = await _context.Wishlists
     .Include(w => w.WishlistItems)
         .ThenInclude(wi => wi.ProductosEmpaque)
             .ThenInclude(pe => pe.ImagenProducto) 
     .Include(w => w.WishlistItems)
         .ThenInclude(wi => wi.ProductosEmpaque)
             .ThenInclude(pe => pe.Producto) 
     .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null) return null;

            return new WishlistResponseDto
            {
                WishlistId = wishlist.Id,
                Items = wishlist.WishlistItems.Select(item => new WishlistProductDto
                {
                    ProductEmpaqueId = item.ProductId,
                    ProductName = item.ProductosEmpaque?.Producto.NombreProducto ?? "NO DATO",
                    Quantity = item.Quantity,
                    ProductoEmpaque = new ProductoEmpaqueDto
                    {
                        ProductoEmpaqueId = item.ProductosEmpaque.Id,
                        ProductoId = item.ProductosEmpaque.ProductoId,
                        EmpaqueId= item.ProductosEmpaque.EmpaqueId,
                        Codigo = item.ProductosEmpaque?.Codigo ,
                          PVenta=item.ProductosEmpaque.PVenta,
                       Descuento= (float?)item.ProductosEmpaque.Descuento,
                        Activo= item.ProductosEmpaque.Activo ,
                        Producto = item.ProductosEmpaque.Producto != null?
                        new ProductoDto
                        {
                          ProductId =item.ProductosEmpaque.Producto.Id,
                                ProductoSatId =item.ProductosEmpaque.Producto.ProductoSatId,
                                Prefijo = item.ProductosEmpaque.Producto.Prefijo,
                                     NombreProducto = item.ProductosEmpaque.Producto.NombreProducto,
                            Descripcion = item.ProductosEmpaque.Producto.Descripcion,
                            DescripcionBreve = item.ProductosEmpaque.Producto.DescripcionBreve,
                            Slug = item.ProductosEmpaque.Producto.Slug,
                            Rating = item.ProductosEmpaque.Producto.Rating,
                            Acumulador = item.ProductosEmpaque.Producto.Acumulador,
                            ProductoIdAcumulador = item.ProductosEmpaque.Producto.ProductoIdAcumulador,
                        } : new ProductoDto(),

                       Empaque= item.ProductosEmpaque.Empaque != null
                        ? new EmpaqueDto
                            {
                                Id = item.ProductosEmpaque.Empaque.Id,
                                Codigo = item.ProductosEmpaque.Empaque.CodigoEmpaque ?? "NO DATO",
                                PCompra = item.ProductosEmpaque.PCompra,
                                PVenta = item.ProductosEmpaque.PVenta,
                                Descuento = item.ProductosEmpaque.Descuento ?? 0,
                                Activo = item.ProductosEmpaque.Activo ?? false,
                                UnidadSat = item.ProductosEmpaque.Empaque.UnidadSAT != null
                                    ? new UnidadSatDto
                                    {
                                        Id = item.ProductosEmpaque.Empaque.UnidadSAT.Id,
                                        ClaveUnidad = item.ProductosEmpaque.Empaque.UnidadSAT.ClaveUnidad ?? "NO DATO",
                                        UnidadSat = item.ProductosEmpaque.Empaque.UnidadSAT.UnidadSat ?? "NO DATO"
                                    }
                                    : null
                           }
                        : new EmpaqueDto(),

                        ImagenProducto = item.ProductosEmpaque.ImagenProducto?.Any() == true
                        ? item.ProductosEmpaque.ImagenProducto.Select(img => new ImageDto
                        {
                            Name = System.IO.Path.GetFileName(img.Url),
                            Url = img.Url,
                            Width = (int)img.Width,
                            Height = (int)img.Height,
                            Formats = new FormatDto()
                        }).ToList()
                        : new List<ImageDto>(),
                    }
                }).ToList()
            };
        }

        public async Task<bool> CreateWishlistIfNotExistsAsync(int userId)
        {
            var wishlist = await _context.Wishlists.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wishlist != null)
                return false;

            wishlist = new Wishlist
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddProductToWishlistAsync(int userId, int productId)
        {

            var productoExiste = await _context.ProductosEmpaque.AnyAsync(p => p.Id == productId);
            if (!productoExiste)
            {
                Console.Write("Producto empaque no existe en BD");
                return false;
            }
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                await CreateWishlistIfNotExistsAsync(userId);
                wishlist = await _context.Wishlists
                    .Include(w => w.WishlistItems)
                    .FirstOrDefaultAsync(w => w.UserId == userId);
            }

            if (wishlist.WishlistItems.Any(wi => wi.ProductId == productId))
                return false; // Ya existe

            wishlist.WishlistItems.Add(new WishlistItem
            {
                ProductId = productId,
                Quantity =1
               
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductFromWishlistAsync(int userId, int productId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
                return false;

            var item = wishlist.WishlistItems.FirstOrDefault(wi => wi.ProductId == productId);
            if (item == null)
                return false;

            wishlist.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearWishlistAsync(int userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
                return false;

            wishlist.WishlistItems.Clear();
            await _context.SaveChangesAsync();
            return true;
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
