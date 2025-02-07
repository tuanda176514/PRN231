namespace JollyAPI.Models.DTOS
{
    public class CartItemData
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Image { get; set; }
    }
}
