namespace JollyAPI.Models.DTOS
{
    public class CheckoutDTO
    {
        public string? Total { get; set; }
        public int? UserId { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public List<CartItemData> OrderDetails { get; set; }
    }
}
