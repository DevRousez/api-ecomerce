using Api_comerce.Dtos;
using Api_comerce.Models;
using Api_comerce.Services.Checkout;
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



    }
}
