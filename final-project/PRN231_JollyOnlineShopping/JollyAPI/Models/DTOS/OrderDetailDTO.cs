using System.ComponentModel.DataAnnotations;

namespace JollyAPI.Models.DTOS
{
	public class OrderDetailDTO
	{
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public decimal? UnitPrice { get; set; }
		public int? Quantity { get; set; }
		public string CustomerName { get; set; }

		public string ProductName { get; set; }
		public string Image { get; set; }
	}
}
