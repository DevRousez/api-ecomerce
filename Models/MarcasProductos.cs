using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    [Table("MARCAS_PRODUCTO")]
    public class MarcaProductos
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? MARCA { get; set; }

        public DateTime? FECHA_CREADO { get; set; }

    }
}
