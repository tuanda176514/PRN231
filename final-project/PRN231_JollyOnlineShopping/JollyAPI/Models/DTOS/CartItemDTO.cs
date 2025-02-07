namespace JollyAPI.Models.DTOS
{
    public class CartItemDTO
    {
        public ProductCartDTO ProductCartDTO { get; set; }
        public int? Quantity { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }

        public CartItemDTO(ProductCartDTO product, int quantity)
        {
            ProductCartDTO = product;
            Quantity = quantity;
        }
    }
}
