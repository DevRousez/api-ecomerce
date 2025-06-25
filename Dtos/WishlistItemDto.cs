using Api_comerce.Models;

namespace Api_comerce.Dtos

{

    public class AddWishlistItemsRequest
    {
        public List<WishlistItemDto> Items { get; set; }
    }

    public class WishlistItemDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int ProductEmpaqueId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotalproduc { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public AccountsDtocs? User { get; set; }
        public ProductoEmpaqueDto? ProductoEmpaque { get; set; }
    }

    public class WishlistItemResultDto
    {
        public int ProductEmpaqueId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public ProductEmpaqueDto ProductEmpaque { get; set; }
    }

    public class UpdateWishlistItemRequest
    {
        public int ProductEmpaqueId { get; set; }
        public int Quantity { get; set; }
    }
}
