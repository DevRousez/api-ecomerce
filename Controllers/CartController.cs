﻿using Api_comerce.Dtos;
using Api_comerce.Models;
using Api_comerce.Services.Cart;
using Api_comerce.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Controllers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("cart")]
    [Authorize] 
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Método helper para obtener userId del token
        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c =>
             c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) throw new Exception("Usuario no encontrado en token");

            return int.Parse(userIdClaim.Value);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var cartItems = await _cartService.GetCartAsync(userId);
            return Ok(new { success = true, data = cartItems });
        }

        
        [HttpPost]
        public async Task<IActionResult> AddItems([FromBody] AddCartItemsRequest request)
        {
            if (request?.Items == null || !request.Items.Any())
                return BadRequest(new { success = false, message = "Productos " });

            var userId = GetUserId();
            var addedItems = await _cartService.AddItemsToCartAsync(userId, request.Items);

            return Ok(new { success = true, data = addedItems });
        }

        // DELETE cart/{productId}
        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var userId = GetUserId();
            var removed = await _cartService.RemoveItemAsync(userId, productId);

            if (!removed) return NotFound(new { success = false, message = "Producto no encontrado en el Carrito" });

            return Ok(new { success = true, message = "Producto eliminado" });
        }

        // DELETE cart/clear
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _cartService.ClearCartAsync(userId);
            return Ok(new { success = true, message = "Carrito vacio" });
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemRequest request)
        {
            // Obtener userId desde el JWT
            var userId = GetUserId();

            if (userId == 0)
                return Unauthorized("Token inválido o faltante.");

            var success = await _cartService.UpdateCartItemAsync(userId, request.ProductEmpaqueId, request.Quantity);

            if (!success)
                return NotFound("Producto no encontrado en el carrito");

            return Ok("Producto actualizado correctamente");
        }


    }

}
