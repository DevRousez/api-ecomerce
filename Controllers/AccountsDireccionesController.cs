using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api_comerce.Dtos;
using Api_comerce.Services;
using Api_comerce.Services.AccountsDirecciones;
using System.Security.Claims;

namespace Api_comerce.Controllers

{
    [ApiController]
    [Route("AccountsDirecciones")]
    [Authorize]
    public class AccountsDireccionesController : ControllerBase
    {
        private readonly IAccountsDireccionesServicie _service;

        public AccountsDireccionesController(IAccountsDireccionesServicie service)
        {
            _service = service;
        }
        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c =>
             c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) throw new Exception("Usuario no encontrado en token");

            return int.Parse(userIdClaim.Value);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetByAccountId()
        {
            int userId = GetUserId();
            var direcciones = await _service.GetByAccountIdAsync(userId);
            return Ok(direcciones);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountDireccionDto dto)
        {
            dto.AccountId = GetUserId();
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountDireccionDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}
