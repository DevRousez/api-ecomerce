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

        public async Task<List<WishlistItemDto>> GetWishlistAsync(int userId)
        {
            var items = await _context.WishlistItems
     .Where(wi => wi.UserId == userId)
     .Select(wi => new WishlistItemDto
     {
         Id = wi.Id,
         UserId = wi.UserId,
         ProductEmpaqueId = wi.ProductId,
         Quantity = wi.Quantity,
         Price = (decimal)wi.ProductEmpaque.PVenta,
         Subtotalproduc = wi.Quantity * wi.Price,
         CreatedAt = wi.CreatedAt,
         UpdatedAt = wi.UpdatedAt,
         User = new AccountsDtocs
         {
             Id = wi.User.Id,
             AccountTypeId = wi.User.AccountTypeId,
             FullName = wi.User.FullName,
             Email = wi.User.Email,
             GoogleId = wi.User.GoogleId,
             Picture = wi.User.Picture,
             IsActive = wi.User.IsActive,
             CreatedAt = wi.User.CreatedAt,
             UpdatedAt = wi.User.UpdatedAt,
         },
         ProductoEmpaque = new ProductoEmpaqueDto
         {
             ProductoEmpaqueId = wi.ProductEmpaque.Id,
             ProductoId = wi.ProductEmpaque.ProductoId,
             EmpaqueId = wi.ProductEmpaque.EmpaqueId,
             Codigo = wi.ProductEmpaque.Codigo,
             PCompra = wi.ProductEmpaque.PCompra,
             PVenta = wi.ProductEmpaque.PVenta,
             Descuento = (float?)wi.ProductEmpaque.Descuento,
             Activo = wi.ProductEmpaque.Activo,
             ImagenProducto = wi.ProductEmpaque.ImagenProducto != null && wi.ProductEmpaque.ImagenProducto.Any()
                 ? wi.ProductEmpaque.ImagenProducto.Select(img => new ImageDto
                 {
                     Name = System.IO.Path.GetFileName(img.Url),
                     Url = img.Url,
                     Width = (int)(img.Width ?? 800),
                     Height = (int)(img.Height ?? 800),
                     Formats = new FormatDto()
                 }).ToList()
                 : new List<ImageDto>(),
             Empaque = new EmpaqueDto
             {
                 Id = wi.ProductEmpaque.Empaque.Id,
                 Empaque = wi.ProductEmpaque.Empaque.Empaque,
                 Contenido = wi.ProductEmpaque.Empaque.Contenido,
                 Sincronizado = wi.ProductEmpaque.Empaque.Sincronizado,
                 CodigoEmpaque = wi.ProductEmpaque.Empaque.CodigoEmpaque,
                 FechaCreado = wi.ProductEmpaque.Empaque.FechaCreado,
                 UnidadSat = null
             },
             Producto = new ProductoDto
             {
                 ProductId = wi.ProductEmpaque.Producto.Id,
                 ProductoSatId = wi.ProductEmpaque.Producto.ProductoSatId,
                 Prefijo = wi.ProductEmpaque.Producto.Prefijo,
                 NombreProducto = wi.ProductEmpaque.Producto.NombreProducto,
                 Slug = wi.ProductEmpaque.Producto.Slug,
                 Rating = wi.ProductEmpaque.Producto.Rating,
                 Acumulador = wi.ProductEmpaque.Producto.Acumulador,
                 ProductoIdAcumulador = wi.ProductEmpaque.Producto.ProductoIdAcumulador,
                 Linea = new LineaDto
                 {
                     Id = (int)wi.ProductEmpaque.Producto.LineaId,
                     Linea = wi.ProductEmpaque.Producto.Linea.Linea,
                     Slug = wi.ProductEmpaque.Producto.Linea.Slug,
                     FechaCreado = wi.ProductEmpaque.Producto.CreatedAt
                 },
                 MarcaProducto = new MarcaProductoDto
                 {
                     Id = wi.ProductEmpaque.Producto.MarcaProducto.Id,
                     Marca = wi.ProductEmpaque.Producto.MarcaProducto.Marca,
                     Slug = wi.ProductEmpaque.Producto.MarcaProducto.Slug
                 }
             }
         }
     })
     .ToListAsync();

            return items;
        }

        public async Task<List<WishlistItemDto>> AddItemsToWishlistAsync(int userId, List<WishlistItemDto> items)
        {
            if (items == null || !items.Any())
                return new List<WishlistItemDto>();

            var productIds = items.Select(i => i.ProductEmpaqueId).ToList();

            var products = await _context.ProductosEmpaque
                .Include(p => p.ImagenProducto)
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            // Obtiene los WishlistItems existentes del usuario para esos productos
            var existingItems = await _context.WishlistItems
                .Where(wi => wi.UserId == userId && productIds.Contains(wi.ProductId))
                .ToDictionaryAsync(wi => wi.ProductId);

            var now = DateTime.UtcNow;

            foreach (var item in items)
            {
                if (!products.TryGetValue(item.ProductEmpaqueId, out var product))
                    continue;

                if (!existingItems.ContainsKey(item.ProductEmpaqueId))
                {
                    var newItem = new WishlistItem
                    {
                        UserId = userId,
                        ProductId = item.ProductEmpaqueId,
                        Quantity = item.Quantity,
                        Price = (decimal)product.PVenta,
                        CreatedAt = now,
                        UpdatedAt = now
                    };
                    _context.WishlistItems.Add(newItem);
                }
                else
                {
                    // Opcional: actualizar cantidad si ya existe
                    var existing = existingItems[item.ProductEmpaqueId];
                    existing.Quantity += item.Quantity;
                    existing.UpdatedAt = now;
                }
            }

            await _context.SaveChangesAsync();

            return await GetWishlistAsync(userId);
        }

        public async Task<bool> RemoveItemAsync(int userId, int productId)
        {
            var item = await _context.WishlistItems
           .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

                if (item == null)
                    return false;

                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();

                return true;
        }

        public async Task<bool> ClearWishlistAsync(int userId)
        {
            var wishlistItem = await _context.WishlistItems
               .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlistItem == null)
                return false;

            _context.WishlistItems.RemoveRange(wishlistItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateWishlistItemAsync(int userId, int productId, int quantity)
        {
            var item = await _context.WishlistItems
             .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (item == null)
                return false;

            item.Quantity = quantity;
            item.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
