using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    [Table("PRODUCTOS")]
    public class Productos
    {
        [Key]
        [Column("PRODUCTO_ID")]
        public int ProductoId { get; set; }

        [Column("PRODUCTO_SAT_ID")]
        public int? ProductoSatId { get; set; }

        [Column("PREFIJO")]
        [StringLength(50)]
        public string? Prefijo { get; set; }

        [Column("PRODUCTO")]
        [StringLength(80)]
        public string? NombreProducto { get; set; }

        [Column("ACUMULADOR")]
        public bool? Acumulador { get; set; }

        [Column("PRODUCTO_ID_ACUMULADOR")]
        public int? ProductoIdAcumulador { get; set; }

        [Column("LINEA_ID")]
        public int? LineaId { get; set; }

        [Column("MARCA_ID")]
        public int? MarcaId { get; set; }

   
        //FK

        [ForeignKey("ProductoSatId")]
        public virtual ProductoSat? ProductoSat { get; set; }

        [ForeignKey("LineaId")]
        public virtual Lineas? Linea { get; set; }

        [ForeignKey("MarcaId")]
        public virtual MarcaProductos? MarcaProducto { get; set; }

        
    }
}
