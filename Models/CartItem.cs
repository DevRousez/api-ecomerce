namespace Api_comerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; } 

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Accounts? User { get; set; }
        public ProductosEmpaque? Product { get; set; }
    }
}
