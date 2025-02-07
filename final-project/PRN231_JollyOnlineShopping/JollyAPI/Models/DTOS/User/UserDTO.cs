namespace JollyAPI.Models.DTOS.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string Phone { get; set; }
        public bool? Status { get; set; }
        public string? Role { get; set; }
    }
}
