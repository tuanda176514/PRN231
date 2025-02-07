namespace JollyAPI.Models.DTOS
{
    public class CartDTO
    {
        public int UserId { get; set; }
        public List<CartItemDTO> Items { get; set; }
    }
}
