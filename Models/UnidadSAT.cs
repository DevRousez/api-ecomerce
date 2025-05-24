using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    [Table("UnidadesSat")]
    public class UnidadSAT
    {
              
            [Key]
            public int Id { get; set; }

            [StringLength(10)]
            public string? ClaveUnidad { get; set; }

            [StringLength(80)]
            public string? UnidadSat { get; set; }

            public DateTime? FechaCreado { get; set; }

          
       
    }
}
