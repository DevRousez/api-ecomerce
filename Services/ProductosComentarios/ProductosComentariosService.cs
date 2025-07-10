using Api_comerce.Data;
using Api_comerce.Dtos;
using Microsoft.EntityFrameworkCore;


namespace Api_comerce.Services.ProductosComentarios
{
    public class ProductosComentariosService
    {
        private readonly AppDbContext _context;

        public ProductosComentariosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductosComentariosDTO>> GetAllAsync()
        {
            var comentarios = await _context.ProductosComentarios
                .Include(c => c.Producto)
                .Include(c => c.Account)
                .ToListAsync();

            return comentarios.Select(c => MapToDto(c)).ToList();
        }

        public async Task<ProductosComentariosDTO?> GetByIdAsync(int id)
        {
            var comentario = await _context.ProductosComentarios
                .Include(c => c.Producto)
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.Id == id);

            return comentario != null ? MapToDto(comentario) : null;
        }

        public async Task<ProductosComentariosDTO> CreateAsync(ProductosComentariosDTO dto)
        {
            bool comproProducto = await _context.OrdenDetalle
     .Include(od => od.OrdenP)
     .AnyAsync(od => od.OrdenP.Id == dto.AccountId && od.ProductoEmpaqueId == dto.ProductoId);

            if (!comproProducto)
            {
                return null; 
            }

            var nuevo = new Api_comerce.Models.ProductosComentarios
            {
                ProductoId = dto.ProductoId,
                AccountId = dto.AccountId,
                Comentario = dto.Comentario,
                Calificacion = dto.Calificacion,
                FechaCreado = DateTime.Now
            };

            _context.ProductosComentarios.Add(nuevo);
            await _context.SaveChangesAsync();

            return MapToDto(nuevo);
        }

        public async Task<bool> UpdateAsync(int id, ProductosComentariosDTO dto)
        {
            var comentario = await _context.ProductosComentarios.FindAsync(id);
            if (comentario == null)
                return false;

            comentario.Comentario = dto.Comentario;
            comentario.Calificacion = dto.Calificacion;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comentario = await _context.ProductosComentarios.FindAsync(id);
            if (comentario == null)
                return false;

            _context.ProductosComentarios.Remove(comentario);
            await _context.SaveChangesAsync();
            return true;
        }

        
        private ProductosComentariosDTO MapToDto(Api_comerce.Models.ProductosComentarios c)
        {
            return new ProductosComentariosDTO
            {
                Id = c.Id,
                ProductoId = c.ProductoId,
                Producto = c.Producto,
                AccountId = c.AccountId,
                Account = c.Account,
                Comentario = c.Comentario,
                Calificacion = c.Calificacion,
                FechaCreado = c.FechaCreado
            };
        }
    }
}
