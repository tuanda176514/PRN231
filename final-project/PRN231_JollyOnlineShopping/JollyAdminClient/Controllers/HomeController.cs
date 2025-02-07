using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using JollyAdminClient.Models;
using JollyAPI.Models.DTOS.Statistic;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace JollyAdminClient.Controllers
{
	public class HomeController : Controller
	{
		private readonly HttpClient client;
		private string productApiUrl;

		public HomeController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			productApiUrl = AppConstant.API_URL + "/products";
		}

		public async Task<IActionResult> Index()
		{
			ViewBag.pageTitle = "Thống kê dữ liệu cửa hàng";
			HttpResponseMessage response = await client.GetAsync(productApiUrl + "/statistics");
			var data = await response.Content.ReadAsStringAsync();
			ViewBag.report = JsonSerializer.Deserialize<StatisticReport>(data, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
			return View();
		}
	}
}