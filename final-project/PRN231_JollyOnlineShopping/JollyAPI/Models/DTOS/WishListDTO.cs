namespace JollyAPI.Models.DTOS
{
    public class WishListDTO
    {
        public int UserId { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
