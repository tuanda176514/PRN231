namespace JollyAPI.Models.DTOS.User
{
	public class PasswordUpdateDTO
	{
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
