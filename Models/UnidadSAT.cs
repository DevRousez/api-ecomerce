using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    [Table("UNIDADES_SAT")]
    public class UnidadSAT
    {
              
            [Key]
            public int Id { get; set; }

            [StringLength(10)]
            public string? CLAVE_UNIDAD { get; set; }

            [StringLength(80)]
            public string? UNIDAD_SAT { get; set; }

            public DateTime? FECHA_CREADO { get; set; }

          
       
    }
}
