using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }

        public int UserId { get; set; } 
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }  
        public decimal Subtotalproduc => Quantity * Price;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("ProductId")]
        public virtual ProductosEmpaque ProductEmpaque { get; set; }

        [ForeignKey("UserId")]
        public virtual Accounts User { get; set; }
    }
}
