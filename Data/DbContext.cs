using Api_comerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Data
{
     public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Productos> Productos { get; set; }
        public DbSet<ProductosEmpaque> ProductosEmpaque { get; set; }
        public DbSet<Empaques> Empaque { get; set; }
        public DbSet<MarcaProductos> MarcaProductos { get; set; }
        public DbSet<UnidadSAT> UnidadSAT { get; set; }
        public DbSet<ProductoSat> ProductoSat { get; set; }
        public DbSet<Lineas> Lineas { get; set; }


    }
}
