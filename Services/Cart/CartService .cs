using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> AddItemsToCartAsync(int userId, List<CartItemDto> items)
        {
            if (items == null || !items.Any()) return new List<CartItem>();

            var productIds = items.Select(i => i.ProductId).ToList();

            var products = await _context.ProductosEmpaque
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var existingCartItems = await _context.CartItems
                .Where(c => c.UserId == userId && productIds.Contains(c.ProductId))
                .ToDictionaryAsync(c => c.ProductId);

            var now = DateTime.UtcNow;
            var result = new List<CartItem>();

            foreach (var item in items)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                    continue; // ignorar productos inválidos

                if (existingCartItems.TryGetValue(item.ProductId, out var existing))
                {
                    existing.Quantity += item.Quantity;
                    existing.UpdatedAt = now;
                    result.Add(existing);
                }
                else
                {
                    var newItem = new CartItem
                    {
                        UserId = userId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = (decimal)product.PVenta, 
                        CreatedAt = now,
                        UpdatedAt = now
                    };

                    _context.CartItems.Add(newItem);
                    result.Add(newItem);
                }
            }

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<CartItem>> GetCartAsync(int userId)
        {
            return await _context.CartItems
                .Include(ci => ci.ProductEmpaque)
                 .ThenInclude(pe => pe.Producto)
                  .Include(ci => ci.ProductEmpaque)
                    .ThenInclude(pe => pe.Empaque)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> RemoveItemAsync(int userId, int productId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var items = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
