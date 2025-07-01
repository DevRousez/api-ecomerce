using System.Security.Claims;
using Api_comerce.Dtos;
using Api_comerce.Models;
using Api_comerce.Services.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Api_comerce.Models.Orden;

namespace Api_comerce.Controllers
{
    [ApiController]
    [Route("orden")]
    public class OrdenController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public OrdenController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) throw new Exception("Usuario no encontrado en token");

            return int.Parse(userIdClaim.Value);
        }
        [Authorize]
        [HttpPost("crear-orden")]
        public async Task<IActionResult> CrearOrden([FromBody] CheckoutRequest dto)
        {
            try
            {
                var ordenId = await _checkoutService.CrearOrdenAsync(dto);

                return Ok(new
                {
                    ordenId,
                    estado = "Pendiente",
                    mensaje = "Orden registrada correctamente."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la orden: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetMisOrdenes()
        {
            try
            {
                int accountId = GetUserId();
                if (accountId == null)
                    return Unauthorized("Usuario no autenticado.");

               
                var ordenes = await _checkoutService.GetOrdenesPorUsuarioAsync(accountId);

                return Ok(ordenes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener órdenes: {ex.Message}");
            }
        }
        [Authorize]
        [HttpGet("{ordenId}")]
        public async Task<IActionResult> GetOrdenByIdAsync(int ordenId)
        {
            var orden = await _checkoutService.GetOrdenByIdAsync(ordenId);
            if (orden == null)
            {
                return NotFound();
            }
            return Ok(orden);
        }
        [Authorize]
        [HttpPut("{ordenId}/marcar-pagada")]
        public async Task<IActionResult> MarcarOrdenComoPagada(int ordenId)
        {
            var resultado = await _checkoutService.MarcarOrdenComoPagadaAsync(ordenId);

            if (!resultado)
            {
                return NotFound(new { mensaje = "Orden no encontrada." });
            }

            return Ok(new { mensaje = "Orden marcada como pagada correctamente." });
        }
        [Authorize]
        [HttpPost("{ordenId}/reenviar-correo")]
        public async Task<IActionResult> ReenviarNotaVenta(int ordenId)
        {
            try
            {
                var reenviado = await _checkoutService.ReenviarNotaVentaCorreoAsync(ordenId);

                if (!reenviado)
                    return NotFound(new { mensaje = "Orden no encontrada o no pudo reenviarse." });

                return Ok(new { mensaje = "Correo reenviado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al reenviar el correo.", detalle = ex.Message });
            }
        }


    }
}
