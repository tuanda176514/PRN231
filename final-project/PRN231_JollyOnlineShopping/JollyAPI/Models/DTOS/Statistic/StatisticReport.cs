using JollyAPI.Models.Entity;

namespace JollyAPI.Models.DTOS.Statistic
{
	public class StatisticReport
	{
		public List<ProductReport> TopProductReport { get; set; }
		public List<Order> RecentOrders { get; set; }
		public int TotalOrders { get; set; }
		public int TotalMonthOrders { get; set; }
		public int TotalProducts { get; set; }
		public float TotalRating { get; set; }
		public double TotalRevenue { get; set; }
	}
}
