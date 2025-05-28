using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Models;
using Api_comerce.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductosController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoEcommerceDto>>> GetProductos()
        {
            var productos = await _productsService.GetAllProductosAsync();
            return Ok(productos);
        }

        [HttpGet("limit")]
        public async Task<ActionResult<IEnumerable<ProductoEcommerceDto>>> GetProductosLimitAsync(
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0
            )
        {
            var productos = await _productsService.GetProductosLimitAsync(limit, offset);

            if (productos == null || !productos.Any())
            {
                return NotFound("No se encontraron productos.");
            }

            return Ok(productos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProductoEcommerceDto>>> GetProducto(int id)
        {
            var producto = await _productsService.GetProductoByIdAsync(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

       
    }
}
