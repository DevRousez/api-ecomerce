using Api_comerce.Dtos;
using Api_comerce.Models;

namespace Api_comerce.Services.Wishlists
{
    public interface IWishlistService
    {
        Task<WishlistResponseDto?> GetWishlistByUserAsync(int userId);
        Task<bool> CreateWishlistIfNotExistsAsync(int userId);
        Task<bool> AddProductToWishlistAsync(int userId, int productId);
        Task<bool> RemoveProductFromWishlistAsync(int userId, int productId);
        Task<bool> ClearWishlistAsync(int userId);
    }
}
