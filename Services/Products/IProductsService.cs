using Api_comerce.Dtos;

namespace Api_comerce.Services.Products
{
    public interface IProductsService
    {
        Task<List<ProductoEcommerceDto>> GetAllProductosAsync();
        Task<ProductoEcommerceDto?> GetProductoByIdAsync(int id);
    }
}
