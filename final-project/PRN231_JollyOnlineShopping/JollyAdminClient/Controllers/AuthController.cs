using JollyAdminClient.Models;
using JollyAPI.Models.DTOS.User;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace JollyAdminClient.Controllers
{
	public class AuthController : Controller
	{
		private readonly HttpClient client;
		private string authUrl;

		public AuthController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			authUrl = AppConstant.API_URL + "/users";
		}
		public async Task<IActionResult> Login()
		{
            HttpResponseMessage response = await client.GetAsync(authUrl + "/1");
            return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(string email, string password)
		{
	
			var response =await client.PostAsJsonAsync(authUrl + "/login", new {email = email ,password = password });
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				UserResponseDTO userResponse = JsonSerializer.Deserialize<UserResponseDTO>(data, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (userResponse != null && userResponse.Role.Equals("admin")) { 
					HttpContext.Session.SetInt32("userId", userResponse.Id);
					return Redirect("/");
				}
			}
			ViewBag.Error = "Email or password is incorrect";
			return View();
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return Redirect("/auth/login");
		}
	}
}
