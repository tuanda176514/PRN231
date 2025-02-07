namespace JollyAPI.Models.DTOS.User
{
    public class AddressesDTO
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
        public int UserId { get; set; }
    }
}
