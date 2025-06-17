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
        public int ProductEmpaqueId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotalproduc { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public AccountsDtocs? User { get; set; }
        public ProductoEmpaqueDto? ProductEmpaque { get; set; }
    }

    public class CartItemResultDto
    {
        public int ProductEmpaqueId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public ProductEmpaqueDto ProductEmpaque { get; set; }
    }

    public class ProductEmpaqueDto
    {
        public int ProductEmpaqueId { get; set; }
        public string Codigo { get; set; }
        public decimal PVenta { get; set; }

        public List<ImagenProductoDto> ImagenProducto { get; set; }
    }

    public class ImagenProductoDto
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
    }
    public class UpdateCartItemRequest
    {
        public int ProductEmpaqueId { get; set; }
        public int Quantity { get; set; }
    }
}
