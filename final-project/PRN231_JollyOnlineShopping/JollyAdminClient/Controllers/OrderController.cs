using JollyAdminClient.Models;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace JollyAdminClient.Controllers
{
	public class OrderController : Controller
	{
		private readonly HttpClient client;
		private string orderApiUrl;
		public List<Order> orders = new List<Order>();
		public List<OrderDetailDTO> orderDetailDTOs = new List<OrderDetailDTO>();

		public OrderController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			orderApiUrl = AppConstant.API_URL + "/orders";
		}

		public async Task<IActionResult> Index()
		{
            HttpResponseMessage response = await client.GetAsync(orderApiUrl);
			var data = await response.Content.ReadAsStringAsync();
			orders = JsonSerializer.Deserialize<List<Order>>(data, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
			ViewBag.orders = orders;
			ViewBag.pageTitle = "Quản lý đơn hàng";
			ViewBag.JsFiles = new List<string> { "list.js", "order.js" };
			return View();
		}


		public async Task<IActionResult> Confirm(int id, string? status)
		{
            ViewBag.JsFiles = new List<string> { "list.js", "order.js" };
            var data = new { Id = id, Status = status };
			HttpResponseMessage response = await client.PutAsJsonAsync($"{orderApiUrl}/status/{id}?status={status}", data);
			await Task.Delay(1000);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Details(int id)
		{
            HttpResponseMessage response = await client.GetAsync($"{orderApiUrl}/detail/{id}");
			var data = await response.Content.ReadAsStringAsync();
			orderDetailDTOs = JsonSerializer.Deserialize<List<OrderDetailDTO>>(data, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
			ViewBag.orderDetailDTOs = orderDetailDTOs;
			ViewBag.pageTitle = "Chi tiết đơn hàng";
			ViewBag.JsFiles = new List<string> { "list.js", "order.js" };
			return View();
		}



	}
}
