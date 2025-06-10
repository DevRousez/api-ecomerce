namespace Api_comerce.Dtos
{
   
    public class AddCartItemsRequest
    {
        public List<CartItemDto> Items { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
