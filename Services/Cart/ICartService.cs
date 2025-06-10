using Api_comerce.Dtos;
using Api_comerce.Models;

namespace Api_comerce.Services.Cart
{
    public interface ICartService
    {
        Task<List<CartItem>> AddItemsToCartAsync(int userId, List<CartItemDto> items);
        Task<List<CartItem>> GetCartAsync(int userId);
        Task<bool> RemoveItemAsync(int userId, int productId);
        Task<bool> ClearCartAsync(int userId);
    }
}
