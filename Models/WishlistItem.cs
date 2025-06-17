using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Wishlist Wishlist { get; set; }
        [ForeignKey("ProductId")]
        public virtual ProductosEmpaque ProductosEmpaque { get; set; } 
    }
}
