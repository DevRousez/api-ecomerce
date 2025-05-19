using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    [Table("LINEAS")]
    public class Lineas
    {
        [Key]
        public int LINEA_ID { get; set; }

        [StringLength(100)]
        public string? LINEA { get; set; }

        public DateTime? FECHA_CREADO { get; set; }

     
    }
}
