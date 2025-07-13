using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_comerce.Models
{
    [Table("ProductosComentarios")]
    public class ProductosComentarios
    {
        [Key]
        public int Id { get; set; }

        public int ProductoEmpaqueId { get; set; }
        public ProductosEmpaque ProductoEmpaque { get; set; }

        public int AccountId { get; set; }
        public Accounts Account { get; set; }

        public string Comentario { get; set; }
        public int Calificacion { get; set; } 
        public DateTime FechaCreado { get; set; } = DateTime.Now;
    }
}
