using Api_comerce.Data;
using Api_comerce.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Api_comerce.Services.Lineas
{
    public class LineasService : ILineasService
    {
        private readonly AppDbContext _context;
       

        public LineasService(AppDbContext context)
        {
            _context = context;
           
        }
        public async Task<List<LineaDto>> GetCategories()
        {
          

            return await _context.Lineas
               
                .Select(lin => new LineaDto
                {
                    Id = lin.Id,
                    Linea = lin.Linea ?? "NO DATO",
                    Slug = lin.Slug,
                    FechaCreado = lin.FechaCreado,

                }).ToListAsync();

        }
        public async Task<LineaDto> GetCategoriesId(int id)
        {


            return  _context.Lineas
                .Where(lin => lin.Id == id)
                .Select(lin => new LineaDto
                {
                    Id = lin.Id,
                    Linea = lin.Linea ?? "NO DATO",
                    Slug = lin.Slug,
                    FechaCreado = lin.FechaCreado,

                }).FirstOrDefault();

        }

        public async Task<List<LineaDto>> GetCategorieslimit(int limit, int offset)
        {


            return await _context.Lineas
                .Skip(offset)
      .Take(limit)
                .Select(lin => new LineaDto
                {
                    Id = lin.Id,
                    Linea = lin.Linea ?? "NO DATO",
                    Slug = lin.Slug,
                    FechaCreado = lin.FechaCreado,

                }).ToListAsync();

        }

    }
}
