using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using JollyCustomerClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace JollyCustomerClient.Controllers
{
    public class OrderController : Controller
    {
		private readonly HttpClient client;
        private string orderUrl;
		public List<Order> orders = new List<Order>();
        public OrderController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			orderUrl = AppConstant.API_URL + "/orders";
		}
		
		public async Task<IActionResult> Index(string from, string to)
		{
			var userId = HttpContext.Session.GetInt32("userId");
            string dateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
            string filter = "";

            if (userId.HasValue)
            {
                filter += "/" + userId;
            }
            
            if (!string.IsNullOrEmpty(from))
            {
                DateTime fromDate = DateTime.Parse(from);
                from = fromDate.ToString(dateTimeFormat);
                filter += "/" + from;
            }

            if (!string.IsNullOrEmpty(to))
            {
                DateTime toDate = DateTime.Parse(to);
                to = toDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(dateTimeFormat);
                filter += "/" + to;
            }
            HttpResponseMessage response = await client.GetAsync(orderUrl +"/myorder" + filter);
            var data = await response.Content.ReadAsStringAsync();
			orders = JsonSerializer.Deserialize<List<Order>>(data, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
			ViewBag.orders = orders.OrderByDescending(x => x.Date).ToList();
			ViewBag.JsFiles = new List<string> { "list.js" };
			return View();
		}

        [HttpGet("orders/myorder/{id}")]
        public IActionResult GetOrders(int id,string from, string to)
        {
           

            return View();
        }
		[Route("/order/detail/{id}")]
		public async Task<IActionResult> Detail(int id)
		{
            HttpResponseMessage response = await client.GetAsync($"{orderUrl}/detail/{id}");
            List<OrderDetailDTO> orderDetailDTOs = new List<OrderDetailDTO>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                orderDetailDTOs = response.Content.ReadFromJsonAsync<List<OrderDetailDTO>>().Result;
            }


            decimal subTotal = 0;

            foreach (var item in orderDetailDTOs)
            {
                decimal itemTotal = (decimal)(item.UnitPrice ?? 0) * (item.Quantity ?? 0);
                subTotal += itemTotal;
            }

            OrderDetailViewModel viewModel = new OrderDetailViewModel
            {
                OrderDetailDTOs = orderDetailDTOs,
                SubTotal = subTotal
            };

            return View(viewModel);
        }

      
    }
}
