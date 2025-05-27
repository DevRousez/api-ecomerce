using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Models
{
    [Table("ProductoEmpaque")]
    public class ProductosEmpaque
    {
        [Key]
        public int Id {get;set;}

        public int ProductoId { get; set; }
        public int EmpaqueId { get; set; }

        [StringLength(20)]
        public string? Codigo { get; set; }
        public decimal? PCompra { get; set; }
        public decimal? PVenta { get; set; }
        public double? Descuento { get; set; }
        public bool? Activo { get; set; }

        // Relaciones de navegación (si tienes):
        [ForeignKey("ProductoId")]
        public virtual Productos Producto { get; set; }

        [ForeignKey("EmpaqueId")]
        public virtual Empaques? Empaque { get; set; }

    }
}
