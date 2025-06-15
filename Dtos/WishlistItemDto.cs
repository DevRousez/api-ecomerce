using Api_comerce.Models;

namespace Api_comerce.Dtos
{
    public class AddWishlistItemsRequest
    {
        public List<WishlistItemDto> Items { get; set; }
    }

    public class WishlistItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class WishlistResponseDto
    {
        public int WishlistId { get; set; }
        public List<WishlistProductDto> Items { get; set; }
    }

    public class WishlistProductDto
    {
        public int ProductEmpaqueId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public virtual ProductoEmpaqueDto ProductoEmpaque { get; set; }
    }
}
