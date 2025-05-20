using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    [Table("PRODUCTOS_SAT")]
    public class ProductoSat
    {
        [Key]
        public int Id { get; set; }

        [Column("CLAVE_PROD")]
        [StringLength(200)]
        public string? ClaveProd { get; set; }

        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }  // varchar(MAX)

        [Column("FECHA_CREADO")]
        public DateTime? FechaCreado { get; set; }

     
    }
}
