using System.Security.Claims;
using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Models;
using Api_comerce.Services.ProductosComentarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Controllers
{
    [ApiController]
    [Route("ReviewsProducts")]
    public class ProductosComentariosController : ControllerBase
    {
        private readonly IProductosComentariosService _comentarioService;

        public ProductosComentariosController(IProductosComentariosService comentarioService)
        {
            _comentarioService = comentarioService;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductosComentariosDTO>>> GetAll()
        {
            var userId = GetUserId();
            var comentarios = await _comentarioService.GetAllAsync(userId);
            return Ok(comentarios);
        }


        [HttpGet("filter")]
        [Authorize]
        public async Task<ActionResult<ProductoConComentariosDTO>> GetByFilter(
    [FromQuery] int? id = null,
    [FromQuery] int? productoEmpaqueId = null)
        {
            if (!id.HasValue && !productoEmpaqueId.HasValue)
                return BadRequest("Debes especificar al menos 'id' o 'productoEmpaqueId'.");
            int? userId = GetUserId();
            if(!userId.HasValue)
                return BadRequest("No autorizado, sesion");
            var comentario = await _comentarioService.GetComentarioFiltradoAsync(id, productoEmpaqueId, userId);
            if (comentario == null)
                return NotFound();

            return Ok(comentario);
        }

        [HttpGet("limit")]
        [Authorize]
        public async Task<ActionResult<ProductosComentariosDTO>> GetByLimit(
    [FromQuery] int? _limit = 10)
        {
            if (!_limit.HasValue)
                return BadRequest("Debes especificar un valor mayor a 0");
            var userId = GetUserId();
            var comentario = await _comentarioService.GetByLimitAsync(_limit, userId);
            if (comentario == null)
                return NotFound();

            return Ok(comentario);
        }



        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductosComentariosDTO>> Create([FromBody] CrearComentarioDTO dto)
        {
            dto.AccountId = GetUserId();    
            var created = await _comentarioService.CreateAsync(dto);
            if (created == null)
                    return BadRequest("El usuario no ha comprado este producto");
           
            
            return CreatedAtAction(nameof(GetByFilter), new { id = created.Id }, created);
        }

        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] ProductosComentariosDTO dto)
        {
            var updated = await _comentarioService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _comentarioService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c =>
             c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) throw new Exception("Usuario no encontrado en token");

            return int.Parse(userIdClaim.Value);
        }

        [HttpGet("AllowAnony/{productoEmpaqueId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductosComentariosDTO>> GetComentariosPorProducto(int productoEmpaqueId)
        {
            var comentarios = await _comentarioService.GetComentariosPorProductoAsync(productoEmpaqueId);
            return Ok(comentarios);
        }
    }
}
