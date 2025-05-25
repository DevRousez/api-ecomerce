using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Models;
using Api_comerce.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoEcommerceDto>> GetProducto(int id)
        {
            var producto = await _productsService.GetProductoByIdAsync(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }
    }
}
