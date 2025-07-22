using System.Collections.Generic;
using Api_comerce.Dtos;

namespace Api_comerce.Services.Products
{
    public interface IProductsService
    {
        Task<List<ProductoEcommerceDto>> GetAllProductosAsync();
        Task<List<ProductoEcommerceDto>> GetProductoByIdAsync(int id);
        Task<List<ProductoEcommerceDto>> GetProductosLimitAsync(int limit = 10, int offset = 0);
        //stored procedures
        Task<List<spProductoEcommerceDto>> GetProductsidSP(int categoryId=0, string slug="null");
        Task<List<spProductoEcommerceDto>> GetProductsSP();

        Task<List<LineaDto>> GetProductosCategoriesSP(int categoryId = 0, string slug = "null");
        //end stored procedures
        Task<List<LineaDto>> GetProductosCategories(int categoryId=0, string slug="null");


        

        Task<List<ProductoEcommerceDto>> GetProductosNameContainsAsync(string? name_contains = null);
        
        //IEnumerable<ProductoEmpaqueDto> GetAllProductCategories();



    }
}
