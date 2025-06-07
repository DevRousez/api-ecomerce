using Api_comerce.Dtos;

namespace Api_comerce.Services.Lineas
{
    public interface ILineasService
    {
       
       Task<LineaDto> GetCategoriesId(int id);
       // Task<List<ProductsCategoriesDTO>> GetCategorieslimit(int limit, int offset);

        
    }
}
