using JollyAPI.Models.Entity;

namespace JollyAPI.Models.DTOS.Statistic
{
	public class ProductReport
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public int Quantity { get; set; }
	}
}
