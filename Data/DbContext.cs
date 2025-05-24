using Api_comerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Data
{
     public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AccountsTypes> AccountsTypes { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<ProductosEmpaque> ProductosEmpaque { get; set; }
        public DbSet<Empaques> Empaque { get; set; }
        public DbSet<MarcaProductos> MarcaProductos { get; set; }
        public DbSet<UnidadSAT> UnidadSAT { get; set; }
        public DbSet<ProductoSat> ProductoSat { get; set; }
        public DbSet<Lineas> Lineas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountsTypes>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<AccountsTypes>().HasData(
                new AccountsTypes { Id = 1, AccountType = "Manager",  IsActive = true },
                new AccountsTypes { Id = 2, AccountType = "Asesor",  IsActive = true },
                new AccountsTypes { Id = 3, AccountType = "Cliente",  IsActive = true }
               
                );
                
            modelBuilder.Entity<Productos>()
                .HasOne(p => p.ProductoSat)
                .WithMany() 
                .HasForeignKey(p => p.ProductoSatId);

            modelBuilder.Entity<Productos>()
                .HasOne(p => p.Linea)
                .WithMany()
                .HasForeignKey(p => p.LineaId);

            modelBuilder.Entity<Productos>()
                .HasOne(p => p.MarcaProducto)
                .WithMany()
                .HasForeignKey(p => p.MarcaId);
        }


     }
}
