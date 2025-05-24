using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_comerce.Models
{
    [Table("Empaques")]
    public class Empaques
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? Empaque { get; set; }

        public double? Contenido { get; set; }

        public bool? Sincronizado { get; set; }

        [StringLength(10)]
        public string? CodigoEmpaque { get; set; }

        public DateTime? FechaCreado { get; set; }

        public int? UnidadId { get; set; }

        // Relación de navegación a Unidad (si tienes esa tabla)
        [ForeignKey(nameof(UnidadId))]
        public virtual UnidadSAT ? UnidadSAT { get; set; }

       
    }
}
