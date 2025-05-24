using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    [Table("MarcasProducto")]
    public class MarcaProductos
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Marca { get; set; }
        public string? Slug { get; set; }

        public DateTime? FechaCreado { get; set; }

    }
}
