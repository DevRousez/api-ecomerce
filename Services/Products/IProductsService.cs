using System.Collections.Generic;
using Api_comerce.Dtos;

namespace Api_comerce.Services.Products
{
    public interface IProductsService
    {
        Task<List<ProductoEcommerceDto>> GetAllProductosAsync();
        Task<List<ProductoEcommerceDto>> GetProductoByIdAsync(int id);
        Task<List<ProductoEcommerceDto>> GetProductosLimitAsync(int limit = 10, int offset = 0);
        Task<List<catLineaDTO>> GetProductsCategories(int categoryId, string slug);
        
        Task<List<ProductoEcommerceDto>> GetProductosNameContainsAsync(string? name_contains = null);
        
        //IEnumerable<ProductoEmpaqueDto> GetAllProductCategories();



    }
}
