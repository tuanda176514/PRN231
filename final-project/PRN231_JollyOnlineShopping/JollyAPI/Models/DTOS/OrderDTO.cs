namespace JollyAPI.Models.DTOS
{
	public class OrderDTO
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public DateTime Date { get; set; }
		public string Status { get; set; }
		public int? UserId { get; set; }
	}
}
