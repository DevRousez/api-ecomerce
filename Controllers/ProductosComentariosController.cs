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
            var comentarios = await _comentarioService.GetAllAsync();
            return Ok(comentarios);
        }

       
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductosComentariosDTO>> GetById(int id)
        {
            var comentario = await _comentarioService.GetByIdAsync(id);
            if (comentario == null)
                return NotFound();

            return Ok(comentario);
        }

       
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductosComentariosDTO>> Create([FromBody] ProductosComentariosDTO dto)
        {
            dto.AccountId = GetUserId();    
            var created = await _comentarioService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
    }
}
