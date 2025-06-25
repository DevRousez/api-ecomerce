using System.Security.Claims;
using Api_comerce.Dtos;
using Api_comerce.Services.Wishlists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_comerce.Controllers
{    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("wishlist")]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) throw new Exception("Usuario no encontrado en token");

            return int.Parse(userIdClaim.Value);
        }

        // GET wishlist
        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            var userId = GetUserId();
            var items = await _wishlistService.GetWishlistAsync(userId);
            return Ok(new { success = true, data = items });
        }

        // POST wishlist
        [HttpPost]
        public async Task<IActionResult> AddItems([FromBody] AddWishlistItemsRequest request)
        {
            if (request?.Items == null || !request.Items.Any())
                return BadRequest(new { success = false, message = "No se enviaron productos" });

            var userId = GetUserId();
            var addedItems = await _wishlistService.AddItemsToWishlistAsync(userId, request.Items);

            return Ok(new { success = true, data = addedItems });
        }

        // DELETE wishlist/{productId}
        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var userId = GetUserId();
            var removed = await _wishlistService.RemoveItemAsync(userId, productId);

            if (!removed)
                return NotFound(new { success = false, message = "Producto no encontrado en la Wishlist" });

            return Ok(new { success = true, message = "Producto eliminado" });
        }

        // DELETE wishlist/clear
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearWishlist()
        {
            var userId = GetUserId();
            await _wishlistService.ClearWishlistAsync(userId);
            return Ok(new { success = true, message = "Wishlist vacía" });
        }

        // PUT wishlist/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateWishlistItem([FromBody] UpdateWishlistItemRequest request)
        {
            var userId = GetUserId();

            if (userId == 0)
                return Unauthorized(new { success = false, message = "Token inválido" });

            var success = await _wishlistService.UpdateWishlistItemAsync(userId, request.ProductEmpaqueId, request.Quantity);

            if (!success)
                return NotFound(new { success = false, message = "Producto no encontrado en la wishlist" });

            return Ok(new { success = true, message = "Producto actualizado correctamente" });
        }
    }
    
    
}
