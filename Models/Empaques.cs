using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_comerce.Models
{
    [Table("EMPAQUES")]
    public class Empaques
    {
        [Key]
        public int EMPAQUE_ID { get; set; }

        [StringLength(50)]
        public string? EMPAQUE { get; set; }

        public double? CONTENIDO { get; set; }

        public bool? SINCRONIZADO { get; set; }

        [StringLength(10)]
        public string? CODIGO_EMPAQUE { get; set; }

        public DateTime? FECHA_CREADO { get; set; }

        public int? UNIDAD_ID { get; set; }

        // Relación de navegación a Unidad (si tienes esa tabla)
        [ForeignKey(nameof(UNIDAD_ID))]
        public virtual UnidadSAT ? UnidadSAT { get; set; }

       
    }
}
