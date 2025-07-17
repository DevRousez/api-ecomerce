using Api_comerce.Dtos;

namespace Api_comerce.Services.ProductosComentarios
{
    public interface IProductosComentariosService
    {
        Task<List<ProductoConComentariosDTO>> GetAllAsync(int? idUser = null);
        Task<ProductoConComentariosDTO?> GetComentarioFiltradoAsync(int? id,int? productoEmpaqueId, int? userId = null);
        Task<List<ProductoConComentariosDTO>> GetByLimitAsync(int? limit = 10, int? userId=0);
        Task<List<ProductoConComentariosDTO>> GetComentariosPorProductoAsync(int productoEmpaqueId);
        Task<ProductosComentariosDTO> CreateAsync(CrearComentarioDTO dto);
        Task<bool> UpdateAsync(int id, ProductosComentariosDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
