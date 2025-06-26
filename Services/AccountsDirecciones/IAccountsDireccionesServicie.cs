using Api_comerce.Dtos;

namespace Api_comerce.Services.AccountsDirecciones
{
    public interface IAccountsDireccionesServicie
    {
        Task<List<AccountDireccionDto>> GetByAccountIdAsync(int accountId);
        Task<AccountDireccionDto> CreateAsync(AccountDireccionDto dto);
        Task<bool> UpdateAsync(int id, AccountDireccionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
