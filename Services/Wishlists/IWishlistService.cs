using Api_comerce.Dtos;
using Api_comerce.Models;

namespace Api_comerce.Services.Wishlists
{
    public interface IWishlistService
    {
        Task<List<WishlistItemDto>> GetWishlistAsync(int userId);
        Task<List<WishlistItemDto>> AddItemsToWishlistAsync(int userId, List<WishlistItemDto> items);
        Task<bool> RemoveItemAsync(int userId, int productId);
        Task<bool> ClearWishlistAsync(int userId);
        Task<bool> UpdateWishlistItemAsync(int userId, int productId, int quantity);
    }
}
