using Api_comerce.Models;

namespace Api_comerce.Dtos
{
   
    public class AddCartItemsRequest
    {
        public List<CartItemDto> Items { get; set; }
    }

    public class CartItemDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public AccountsDtocs? User { get; set; }
        public ProductoEmpaqueDto? ProductEmpaque { get; set; }
    }
}
