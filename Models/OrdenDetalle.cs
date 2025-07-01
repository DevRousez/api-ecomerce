using static Api_comerce.Models.Orden;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    public class OrdenDetalle
    {
        [Key]
        public int Id { get; set; }

        public int OrdenId { get; set; }
        [ForeignKey("OrdenId")]
        public Orden OrdenP { get; set; }

        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        [ForeignKey("ProductoId")]
        public ProductosEmpaque ProductoEmpaque { get; set; } 
    }
}
