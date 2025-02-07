using JollyAdminClient.Models;
using JollyAPI.DAO;
using JollyAPI.Models.DTOS.User;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace JollyAdminClient.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient client;
        private string userApiUrl;
        public List<User> users = new List<User>();

        public UserController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            userApiUrl = AppConstant.API_URL + "/users";
        }

        public async Task<IActionResult> Index()
        {
            ViewData["JsFiles"] = new List<string>() { "user/toastUser.js" };
            HttpResponseMessage response = await client.GetAsync(userApiUrl + "/get-all");
            var data = await response.Content.ReadAsStringAsync();
            users = JsonSerializer.Deserialize<List<User>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            ViewBag.users = users;
            ViewBag.pageTitle = "Quản lý người dùng ";
            ViewBag.JsFiles = new List<string> { "list.js" };
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync(userApiUrl + "/" + id);

            User users = new User();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                users = response.Content.ReadFromJsonAsync<User>().Result;
            }
            ViewBag.pageTitle = "Chi tiết người dùng " + users.FullName;
            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Add(UserDTO userDTO)
        {
            ViewData["JsFiles"] = new List<string>() { "user/toastUser.js" };

            var httpContent = new StringContent(JsonSerializer.Serialize(userDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(userApiUrl + "/register", httpContent);

            ViewBag.pageTitle = "Thêm người dùng ";

            if (response.IsSuccessStatusCode)
            {
                return Redirect("/User/Create?successCreate");
            }
            else
            {
                return Redirect("/User/Create?errCreate");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewData["JsFiles"] = new List<string>() { "user/toastUser.js" };
            HttpResponseMessage response = await client.GetAsync(userApiUrl + "/" + id);
            UserDTO userDTO = await response.Content.ReadFromJsonAsync<UserDTO>();
            ViewBag.pageTitle = "Chỉnh sửa người dùng ";
            return View(userDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserDTO userDTO)
        {
            try
            {
                ViewData["JsFiles"] = new List<string>() { "user/toastUser.js" };

                var httpContent = new StringContent(JsonSerializer.Serialize(userDTO), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(userApiUrl + "/" + userDTO.Id, httpContent);
                ViewBag.pageTitle = "Chỉnh sửa người dùng ";
                if (response.IsSuccessStatusCode)
                {
                    return Redirect($"/User/Update/{userDTO.Id}?successUpdate");
                }
                else
                {
					ModelState.AddModelError(string.Empty, "Failed to update the user.");
					return View(userDTO);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(userDTO);
            }
        }

        public async Task<IActionResult> ChangeStatus(int id, UserDTO userDTO)
        {
			ViewData["JsFiles"] = new List<string>() { "user/toastUser.js" };
			var httpContent = new StringContent(JsonSerializer.Serialize(userDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(userApiUrl + "/changeStatus" + "/" + id, httpContent);
            ViewBag.pageTitle = "Chỉnh sửa người dùng ";
            if (response.IsSuccessStatusCode)
            {
                return Redirect("/User?successChangeStatus");
            }
            return View(userDTO);
        }
    }
}
