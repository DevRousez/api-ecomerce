using Api_comerce.Data;
using Api_comerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productos>>> GetProductos()
        {
            return await _context.Productos
                .Include(p => p.ProductoSat)
                .Include(p => p.MarcaProducto)
                .Include(p => p.Linea)
            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Productos>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.ProductoSat)
                .Include(p => p.MarcaProducto)
                .Include(p => p.Linea)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }
    }
}
