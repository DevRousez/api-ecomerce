using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_comerce.Models
{
    [Table("ProductosCaracteristicas")]
    public class ProductosCaracteristicas
    {
        [Key]
        public int id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreado { get; set; }
    }
}
