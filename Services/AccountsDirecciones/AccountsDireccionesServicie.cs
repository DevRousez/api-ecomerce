using Api_comerce.Data;
using Api_comerce.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Services.AccountsDirecciones
{
    public class AccountsDireccionesServicie : IAccountsDireccionesServicie
    {
        private readonly AppDbContext _context;

        public AccountsDireccionesServicie(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountDireccionDto>> GetByAccountIdAsync(int accountId)
        {
            return await _context.AccountsDirecciones
                .Where(d => d.AccountId == accountId)
                .Select(d => new AccountDireccionDto
                {
                    Id = d.Id,
                    AccountId = d.AccountId,
                    Calle = d.Calle,
                    Ciudad = d.Ciudad,
                    Estado = d.Estado,
                    CodigoPostal = d.CodigoPostal,
                    Pais = d.Pais,
                    Telefono = d.Telefono,
                    Tipo = d.Tipo,
                    EsPredeterminada = d.EsPredeterminada
                })
                .ToListAsync();
        }

        public async Task<AccountDireccionDto> CreateAsync(AccountDireccionDto dto)
        {


            if (dto.EsPredeterminada)
            {
                var direccionesExistentes = await _context.AccountsDirecciones
                    .Where(d => d.AccountId == dto.AccountId && d.EsPredeterminada)
                    .ToListAsync();

                foreach (var dir in direccionesExistentes)
                {
                    dir.EsPredeterminada = false;
                    dir.UpdatedAt = DateTime.UtcNow;
                }

                // Guarda cambios de los existentes antes de agregar la nueva
                await _context.SaveChangesAsync();
            }

            var direccion = new Models.AccountsDirecciones
            {
                AccountId = dto.AccountId,
                Calle = dto.Calle,
                Ciudad = dto.Ciudad,
                Estado = dto.Estado,
                CodigoPostal = dto.CodigoPostal,
                Pais = dto.Pais,
                Telefono = dto.Telefono,
                Tipo = dto.Tipo,
                EsPredeterminada = dto.EsPredeterminada,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AccountsDirecciones.Add(direccion);
            await _context.SaveChangesAsync();

            dto.Id = direccion.Id;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, AccountDireccionDto dto)
        {
            var direccion = await _context.AccountsDirecciones.FindAsync(id);
            if (direccion == null) return false;

            direccion.Calle = dto.Calle;
            direccion.Ciudad = dto.Ciudad;
            direccion.Estado = dto.Estado;
            direccion.CodigoPostal = dto.CodigoPostal;
            direccion.Pais = dto.Pais;
            direccion.Telefono = dto.Telefono;
            direccion.Tipo = dto.Tipo;
            direccion.EsPredeterminada = dto.EsPredeterminada;
            direccion.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var direccion = await _context.AccountsDirecciones.FindAsync(id);
            if (direccion == null) return false;

            _context.AccountsDirecciones.Remove(direccion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
