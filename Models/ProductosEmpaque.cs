using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Models
{
    [PrimaryKey(nameof(ProductoId), nameof(EmpaqueId))]
    [Table("PRODUCTO_EMPAQUE")]
    public class ProductosEmpaque
    {
        [Column("PRODUCTO_ID")]
        public int ProductoId { get; set; }

        [Column("EMPAQUE_ID")]
        public int EmpaqueId { get; set; }

        [Column("CODIGO")]
        [StringLength(20)]
        public string? Codigo { get; set; }

        [Column("PCOMPRA")]
        public decimal? PCompra { get; set; }

        [Column("PVENTA")]
        public decimal? PVenta { get; set; }

        [Column("DESCUENTO")]
        public double? Descuento { get; set; }

        [Column("ACTIVO")]
        public bool? Activo { get; set; }

        // Relaciones de navegación (si tienes):
        public virtual Productos Producto { get; set; } = null!;
        public virtual Empaques Empaque { get; set; } = null!;
 

}
}
