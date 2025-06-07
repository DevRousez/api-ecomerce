using Api_comerce.Dtos;
using Api_comerce.Services.Lineas;
using Api_comerce.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Api_comerce.Controllers
{
    [ApiController]
 
    public class LineasController : ControllerBase
    {
        private readonly ILineasService _lineasService;

        public LineasController(ILineasService lineasService)
        {
            _lineasService = lineasService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<LineaDto>> GetCategoriesId(int id)
        {
            var categories = await _lineasService.GetCategoriesId(id);
            return Ok(categories);
        }

        //[HttpGet("limit")]
        //public async Task<ActionResult<IEnumerable<ProductoEcommerceDto>>> GetProductosLimitAsync(
        //    [FromQuery] int limit = 10,
        //    [FromQuery] int offset = 0
        //    )
        //{
        //    var categories = await _lineasService.GetCategorieslimit(limit, offset);

        //    if (categories == null || !categories.Any())
        //    {
        //        return NotFound("No se encontraron categorias.");
        //    }

        //    return Ok(categories);
        //}
    }
}
