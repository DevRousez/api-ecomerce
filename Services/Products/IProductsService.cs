using System.Collections.Generic;
using Api_comerce.Dtos;

namespace Api_comerce.Services.Products
{
    public interface IProductsService
    {
        Task<List<ProductoEcommerceDto>> GetAllProductosAsync();
        Task<List<ProductoEcommerceDto>> GetProductoByIdAsync(int id);
        Task<List<ProductoEcommerceDto>> GetProductosLimitAsync(int limit = 10, int offset = 0);



    }
}
