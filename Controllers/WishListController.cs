using System.Security.Claims;
using Api_comerce.Dtos;
using Api_comerce.Services.Wishlists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_comerce.Controllers
{
    [ApiController]
    [Route("WishList")]
    public class WishListController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishListController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetWishlist()
        {
            int userId = GetUserId();
            var wishlist = await _wishlistService.GetWishlistByUserAsync(userId);
            if (wishlist == null)
                return NotFound("Wishlist no Encontrado");
            return Ok(wishlist);
        }
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddToWishlist([FromBody] WishlistItemDto dto)
        {
            int userId = GetUserId();
            var result = await _wishlistService.AddProductToWishlistAsync(userId, dto.ProductId);
            if (!result) return BadRequest("Producto ya existe en wishlist");
            return Ok("Producto añadido a wishlist");
        }
        [Authorize]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromWishlist([FromBody] WishlistItemDto dto)
        {
            int userId = GetUserId();
            var result = await _wishlistService.RemoveProductFromWishlistAsync(userId, dto.ProductId);
            if (!result) return NotFound("Producto no encontrado en wishlist");
            return Ok("Product eliminado de wishlist");
        }
        [Authorize]
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearWishlist()
        {
            int userId = GetUserId();
            var result = await _wishlistService.ClearWishlistAsync(userId);
            if (!result) return NotFound("Wishlist no encontrado");
            return Ok("Wishlist limpiado");
        }
    }
}
