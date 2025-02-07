
using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.DTOS.User;
using JollyAPI.Models.Entity;
using JollyAPI.Services;
using JollyCustomerClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static NuGet.Packaging.PackagingConstants;

namespace JollyCustomerClient.Controllers
{
	public class CartController : Controller
	{

        private readonly HttpClient client = null;
		private string cartApiUrl;
		public string productApiUrl;
		public string orderApiUrl;
		public string userApiUrl;

		List<CartItemData> listItems = null;
		List<Address> listAddresses = null;

        public CartController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			cartApiUrl = AppConstant.API_URL + "/cart";
			productApiUrl = AppConstant.API_URL + "/products";
            userApiUrl = AppConstant.API_URL + "/users";
            orderApiUrl = AppConstant.API_URL + "/orders";
			listItems = new List<CartItemData>();
            listAddresses = new List<Address>();
		}
        public async Task<IActionResult> Index()
        {

			ViewData["JsFiles"] = new List<string>() { "/cart/add.js" };
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId.HasValue)
            {
                userId = userId.Value;
                HttpResponseMessage responseCart = await client.GetAsync(cartApiUrl + "/" + userId);
                string strData = await responseCart.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                listItems = System.Text.Json.JsonSerializer.Deserialize<List<CartItemData>>(strData, options);
                ViewBag.listItems = listItems;
            }
            else
            {
                var cartItemsInSession = HttpContext.Session.GetString("cartItems");
                if (!string.IsNullOrEmpty(cartItemsInSession))
                {
                    listItems = System.Text.Json.JsonSerializer.Deserialize<List<CartItemData>>(cartItemsInSession);
                }
                else
                {
                    listItems = new List<CartItemData>();
                }
            }
            ViewBag.listItems = listItems;
            return View();
        }


        public async Task<IActionResult> CheckoutAsync()
        {
            ViewData["JsFiles"] = new List<string>() { "/cart/add.js" };
            int userId = (int)HttpContext.Session.GetInt32("userId");
            HttpResponseMessage responseUser = await client.GetAsync(userApiUrl + "/" + userId);
            HttpResponseMessage responseAddress = await client.GetAsync(userApiUrl + "/address/" + userId);
            string strUserData = await responseUser.Content.ReadAsStringAsync();
            string strAddressData = await responseAddress.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            listAddresses = System.Text.Json.JsonSerializer.Deserialize<List<Address>>(strAddressData, options);
            if (listAddresses.Count == 0)
            {
                return Redirect("/User/CreateAddress?errNullAddress");
            }
            User user = System.Text.Json.JsonSerializer.Deserialize<User>(strUserData, options);
			ViewBag.FullName = user.FullName;
			ViewBag.Street = user.Addresses.First().Street + " - " + user.Addresses.First().Ward;
			ViewBag.District = user.Addresses.First().District;
			ViewBag.City = user.Addresses.First().City;
			ViewBag.Phone = user.Phone;
			ViewBag.Email = user.Email;

            HttpResponseMessage responseCart = await client.GetAsync(cartApiUrl + "/" + userId);
            string strData = await responseCart.Content.ReadAsStringAsync();

            listItems = System.Text.Json.JsonSerializer.Deserialize<List<CartItemData>>(strData, options);
            ViewBag.listItems = listItems;
            ViewBag.listAddresses = listAddresses;
            return View();
        }

        public async Task<IActionResult> Order()
        {
            ViewData["JsFiles"] = new List<string>() { "/cart/add.js" };
            int userId = (int)HttpContext.Session.GetInt32("userId");
            HttpResponseMessage responseCart = await client.GetAsync(cartApiUrl + "/" + userId);
            string strData = await responseCart.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            listItems = System.Text.Json.JsonSerializer.Deserialize<List<CartItemData>>(strData, options);
            var checkoutDTO = new CheckoutDTO
            {
                UserId = userId,
                CustomerName = Request.Form["FullName"],
                Phone = Request.Form["Phone"],
                Total = Request.Form["Total"],
                Address = Request.Form["Address"],
                OrderDetails = listItems
            };
			ViewBag.listAddresses = listAddresses;
			var httpContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(checkoutDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(orderApiUrl + "/checkout", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("/Order");
            }
            return View("/Order");
        }


    }
}

