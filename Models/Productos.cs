using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    [Table("Productos")]
    public class Productos
    {
        [Key]
        public int Id { get; set; }
        public int? ProductoSatId { get; set; }
        [StringLength(50)]
        public string? Prefijo { get; set; }
        [StringLength(80)]
        public string? NombreProducto { get; set; }
        [StringLength(300)]
        public string? Descripcion { get; set; }
        [StringLength(300)]
        public string? DescripcionBreve { get; set; }
        public string? Slug { get; set; }
        public int? Rating { get; set; }
        public bool? Acumulador { get; set; }
        public int? ProductoIdAcumulador { get; set; }
        public int? LineaId { get; set; }
        public int? MarcaId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        //FK

        [ForeignKey("ProductoSatId")]
        public virtual ProductoSat? ProductoSat { get; set; }

        [ForeignKey("LineaId")]
        public virtual Lineas? Linea { get; set; }

        [ForeignKey("MarcaId")]
        public virtual MarcaProductos? MarcaProducto { get; set; }


        public virtual ICollection<ProductosEmpaque>? ProductosEmpaque { get; set; }

        [ForeignKey("UnidadId")]
        public virtual UnidadSAT? UnidadSAT { get; set; }
    }
}
