using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_comerce.Dtos;

namespace Api_comerce.Models
{
    [Table("Lineas")]
    public class Lineas
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Linea { get; set; }
        public string? Slug { get; set; }
        public DateTime? FechaCreado { get; set; }
        public List<Productos> Productos { get; set; } = new();

    }
}
