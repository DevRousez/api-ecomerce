using Api_comerce.Dtos;

namespace Api_comerce.Services.ProductosComentarios
{
    public interface IProductosComentariosService
    {
        Task<List<ProductosComentariosDTO>> GetAllAsync();
        Task<ProductosComentariosDTO?> GetByIdAsync(int? id,int? productoEmpaqueId);
    
        Task<ProductosComentariosDTO> CreateAsync(CrearComentarioDTO dto);
        Task<bool> UpdateAsync(int id, ProductosComentariosDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
