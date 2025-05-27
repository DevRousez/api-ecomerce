using Api_comerce.Dtos;

namespace Api_comerce.Services.Lineas
{
    public interface ILineasService
    {
        Task<List<LineaDto>> GetCategories();
        Task<LineaDto> GetCategoriesId(int id);
        Task<List<LineaDto>> GetCategorieslimit(int limit, int offset);

        
    }
}
