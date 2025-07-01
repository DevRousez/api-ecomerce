using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_comerce.Models
{
    [Table("ProductosCaracteristicas")]
    public class ProductosCaracteristicas
    {
        [Key]
        public int Id { get; set; }

        public int Productoid { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }  // Ej: "Color", "Acabado", etc.

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; } // Ej: "Rojo", "Mate", etc.

        public DateTime FechaCreado { get; set; } = DateTime.Now;

        [ForeignKey("Productoid")]
        public virtual Productos Producto { get; set; }
    }
}
