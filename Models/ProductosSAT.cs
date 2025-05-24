using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    [Table("ProductosSat")]
    public class ProductoSat
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string? ClaveProd { get; set; }
        public string? Descripcion { get; set; }  // varchar(MAX)
        public DateTime? FechaCreado { get; set; }

     
    }
}
