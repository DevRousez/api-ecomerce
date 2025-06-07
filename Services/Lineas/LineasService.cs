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
       
        public async Task<LineaDto> GetCategoriesId(int id)
        {


            return _context.Lineas
                .Where(lin => lin.Id == id)
                .Select(lin => new LineaDto
                {
                    Id = lin.Id,
                    Linea = lin.Linea ?? "NO DATO",
                    Slug = lin.Slug,
                    FechaCreado = lin.FechaCreado,

                }).FirstOrDefault();

        }

        //public async Task<List<ProductsCategoriesDTO>> GetCategorieslimit(int limit, int offset)
        //{


        //    return new ProductsCategoriesDTO[];

        //}

    }
}
