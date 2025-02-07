using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using JollyCustomerClient.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using JollyAPI.Models.DTOS.User;
using JollyAPI.DAO;
using System.Text;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace JollyCustomerClient.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient client;
        private string loginUrl;
        public UserController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            loginUrl = AppConstant.API_URL + "/users";
        }


        public IActionResult Login()
        {
            ViewData["JsFiles"] = new List<string>() { "user/loginToast.js" };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            ViewData["JsFiles"] = new List<string>() { "user/loginToast.js","/cart/add.js" };
            var response = await client.PostAsJsonAsync(loginUrl + "/login", new { email = email, password = password });
            var rp = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserResponseDTO userResponse = JsonSerializer.Deserialize<UserResponseDTO>(data, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
               
                if (userResponse != null && userResponse.Role.Equals("user"))
                {
                    HttpContext.Session.SetInt32("userId", userResponse.Id);
                    return Redirect("/Home?success");
                }
            }
            else if (response.StatusCode.ToString().Equals("BadRequest"))
            {
                return Redirect("/User/Login?loginErrorStatus");
            }
            else
            {
                
                ViewBag.Error = "Email or password is incorrect";
                return Redirect("/User/Login?loginError");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            ViewData["JsFiles"] = new List<string>() { "user/login.js" };
            var httpContent = new StringContent(JsonSerializer.Serialize(userDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(loginUrl + "/register", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                if (response != null)
                {
                    return Redirect("/User/Login?successRegister");
                }
            }
            else
            {
                ViewBag.Error = "Email duplicate";
                return Redirect("/User/Login?errorRegister");
            }
           
            return View("Login");
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            string randomPassword = null;


            HttpResponseMessage response = await client.PostAsJsonAsync(loginUrl + "/resetpassword", email);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                randomPassword = responseData;
            }

            if (!string.IsNullOrEmpty(randomPassword))
            {
                HttpContext.Session.SetString("RandomPassword", randomPassword);
                return Redirect("/User/Login");
            }
            else
            {
                ViewBag.Error = "User not found or password reset failed.";
                return View();
            }
        }

        public async Task<IActionResult> MyProfile(int id)
        {
			ViewData["JsFiles"] = new List<string>() { "user/login.js", "user/updateProfile.js" };
			HttpResponseMessage response = await client.GetAsync(loginUrl + "/" + id);

            User users = new User();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                users = response.Content.ReadFromJsonAsync<User>().Result;
            }
            ViewBag.pageTitle = "Chi tiết người dùng " + users.FullName;
            return View(users);
        }

        public async Task<IActionResult> UpdateProfile(int id)
        {
            ViewData["JsFiles"] = new List<string>() { "user/updateProfile.js" };

            HttpResponseMessage response = await client.GetAsync(loginUrl + "/" + id);

            User users = new User();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                users = response.Content.ReadFromJsonAsync<User>().Result;
            }
            ViewBag.pageTitle = "Chi tiết người dùng " + users.FullName;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserDTO userDTO)
        {
            ViewData["JsFiles"] = new List<string>() {  "user/updateProfile.js" };
            try
            {

                var httpContent = new StringContent(JsonSerializer.Serialize(userDTO), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(loginUrl + "/" + userDTO.Id, httpContent);
                ViewBag.pageTitle = "Chỉnh sửa người dùng ";
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.Remove("userId");
					return Redirect("/User/UpdateProfile?updateProfileSuccess");
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

        public async Task<IActionResult> ChangeAddress(int id)
        {
            ViewData["JsFiles"] = new List<string>() { "user/updateProfile.js" };
            HttpResponseMessage response = await client.GetAsync(loginUrl + "/address/" + id);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<Address> addresses = response.Content.ReadFromJsonAsync<List<Address>>().Result;

                ViewBag.pageTitle = "Chi tiết người dùng";

                return View(addresses);
            }

            return NotFound("User not found");
        }

        [HttpGet]
        public async Task<IActionResult> EditAddress()
        {
            ViewData["JsFiles"] = new List<string>() { "user/updateProfile.js" };
            var userId = HttpContext.Request.Query["userId"];
            var id = HttpContext.Request.Query["id"];

            HttpResponseMessage response = await client.GetAsync(loginUrl + "/address/" + userId + "/" + id);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                AddressesDTO addresses = response.Content.ReadFromJsonAsync<AddressesDTO>().Result;

                ViewBag.pageTitle = "Chi tiết người dùng";

                return View(addresses);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(AddressesDTO addresses)
        {
            try
            {
                ViewData["JsFiles"] = new List<string>() { "user/updateProfile.js" };
                HttpResponseMessage response = await client.GetAsync(loginUrl + "/address/" + addresses.UserId + "/" + addresses.Id);
                Address existingAddress = null;

                if (response.IsSuccessStatusCode)
                {
                    existingAddress = await response.Content.ReadFromJsonAsync<Address>();
                }

                if (existingAddress != null)
                {
                    if (addresses.City == null)
                    {
                        addresses.City = existingAddress.City;
                    }

                    if (addresses.District == null)
                    {
                        addresses.District = existingAddress.District;
                    }

                    if (addresses.Ward == null)
                    {
                        addresses.Ward = existingAddress.Ward;
                    }
                }

                var httpContent = new StringContent(JsonSerializer.Serialize(addresses), Encoding.UTF8, "application/json");

                response = await client.PutAsync(loginUrl + "/address/" + addresses.UserId + "/" + addresses.Id, httpContent);
                ViewBag.pageTitle = "Chỉnh sửa người dùng ";

                if (response.IsSuccessStatusCode)
                {
                    return Redirect("/User/MyProfile?updateAddressSuccess");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to update the address.");
                    return View(addresses);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(addresses);
            }
        }


        public IActionResult CreateAddress()
        {
            ViewData["JsFiles"] = new List<string>() { "user/address.js" };
            return View(); 
        }

        public async Task<IActionResult> DeleteAddress()
        {
            ViewData["JsFiles"] = new List<string>() { "user/updateProfile.js" };
            var userId = HttpContext.Request.Query["userId"];
            var id = HttpContext.Request.Query["id"];

            HttpResponseMessage response = await client.DeleteAsync(loginUrl + "/address/" + id + "/" + userId);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return Redirect($"/User/MyProfile?deleteAddressSuccess");
            }
            return BadRequest();

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userId");

            return Redirect("/");
        }
    }
}