using Api_comerce.Data;
using Api_comerce.Dtos;
using Microsoft.EntityFrameworkCore;


namespace Api_comerce.Services.ProductosComentarios
{
    public class ProductosComentariosService : IProductosComentariosService
    {
        private readonly AppDbContext _context;

        public ProductosComentariosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductoConComentariosDTO>> GetAllAsync(int? idUser = null)
        {
            var comentarios = await _context.ProductosComentarios
                .Include(c => c.ProductoEmpaque)
                    .ThenInclude(p => p.Producto)
                .Include(c => c.Account)
                .OrderByDescending(c => c.FechaCreado)
                .ToListAsync();

            var agrupados = comentarios
                .GroupBy(c => c.ProductoEmpaqueId)
                .Select(g =>
                {
                    var productoEmpaque = g.First().ProductoEmpaque;
                    var producto = productoEmpaque.Producto;

                    return new ProductoConComentariosDTO
                    {
                        ProductoEmpaqueId = productoEmpaque.Id,
                        ProductoId = producto.Id,
                        NombreProducto = producto.NombreProducto ?? "Sin nombre",

                        Comentarios = g.Select(c => MapToDto(c, idUser)).ToList()
                    };
                })
                .ToList();

            return agrupados;
        }

        public async Task<ProductoConComentariosDTO?> GetComentarioFiltradoAsync(int? id = null, int? productoEmpaqueId = null, int? idUser = null)
        {
            var query = _context.ProductosComentarios
                .Include(c => c.ProductoEmpaque)
                    .ThenInclude(p => p.Producto)
                .Include(c => c.Account)
                .AsQueryable();

            if (id.HasValue)
                query = query.Where(c => c.Id == id.Value);

            if (productoEmpaqueId.HasValue)
                query = query.Where(c => c.ProductoEmpaqueId == productoEmpaqueId.Value);

            var comentario = await query.FirstOrDefaultAsync();

            if (comentario == null)
                return null;

            var productoEmpaque = comentario.ProductoEmpaque;
            var producto = productoEmpaque.Producto;

            return new ProductoConComentariosDTO
            {
                ProductoEmpaqueId = productoEmpaque.Id,
                ProductoId = producto.Id,
                NombreProducto = producto.NombreProducto,
              
                Comentarios = new List<ProductosComentariosDTO>
                    {
                        MapToDto(comentario, idUser)
                    }
            };
        }
        public async Task<List<ProductoConComentariosDTO>> GetByLimitAsync(int? limit = 10, int? userId=0)
        {
            var comentarios = await _context.ProductosComentarios
                .Include(c => c.ProductoEmpaque)
                    .ThenInclude(e => e.Producto)
                .Include(c => c.Account)
                .OrderByDescending(c => c.FechaCreado)
                .Take(limit ?? 10)
                .ToListAsync();

            var agrupados = comentarios
                .GroupBy(c => c.ProductoEmpaqueId)
                .Select(g =>
                {
                    var productoEmpaque = g.First().ProductoEmpaque;
                    var producto = productoEmpaque.Producto;

                    return new ProductoConComentariosDTO
                    {
                        ProductoEmpaqueId = productoEmpaque.Id,
                        ProductoId = producto.Id,
                        NombreProducto = producto.NombreProducto ?? "Sin nombre",
                        
                        Comentarios = g.Select(c => MapToDto(c, userId)).ToList()
                    };
                })
                .ToList();

            return agrupados;
        }

        public async Task<ProductosComentariosDTO> CreateAsync(CrearComentarioDTO dto)
        {
            bool comproProducto = await _context.OrdenDetalle
      .Include(od => od.OrdenP)
          .ThenInclude(o => o.AccountsDireccion)
      .AnyAsync(od => od.ProductoEmpaqueId == dto.ProductoId
                      && od.OrdenP.AccountsDireccion.AccountId == dto.AccountId);

            if (!comproProducto)
            {
                return null; 
            }

            var nuevo = new Api_comerce.Models.ProductosComentarios
            {
                ProductoEmpaqueId = dto.ProductoId,
                AccountId = dto.AccountId,
                Comentario = dto.Comentario,
                Calificacion = dto.Calificacion,
                FechaCreado = DateTime.Now
            };

            _context.ProductosComentarios.Add(nuevo);
            await _context.SaveChangesAsync();

            return MapToDto(nuevo, dto.AccountId);
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

        
        private ProductosComentariosDTO MapToDto(Api_comerce.Models.ProductosComentarios c,int ? idUset)
        {
            return new ProductosComentariosDTO
            {
                Id = c.Id,
               
              canEdit = idUset.HasValue && c.AccountId == idUset.Value,
                AccountId = c.AccountId,
                Account = new AccountDto
                {
                    Id = c.Account.Id,
                    FullName = c.Account.FullName
                },
                Comentario = c.Comentario,
                Calificacion = c.Calificacion,
                FechaCreado = c.FechaCreado,
                 ProductoEmpaque = new ProductoEmpaqueDto
                 {
                     ProductoEmpaqueId = c.ProductoEmpaque.Id,
                     ProductoId = c.ProductoEmpaque.ProductoId,
                     Codigo = c.ProductoEmpaque.Codigo
                 }

            };
        }

        //public async Task<ProductosComentariosDTO?> GetByProductosIdAsync(int ProductoEmpaqueid)
        //{
        //    var comentario = await _context.ProductosComentarios
        //        .Include(c => c.Producto)
        //        .Include(c => c.Account)
        //        .FirstOrDefaultAsync(c => c.ProductoId == ProductoEmpaqueid);

        //    return comentario != null ? MapToDto(comentario) : null;
        //}
    }
}
