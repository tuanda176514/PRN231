using JollyAdminClient.Models;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.DTOS.User;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JollyAdminClient.Controllers
{

    public class BlogController : Controller
    {
        private readonly HttpClient client;
        private string blogApiUrl;
        public List<Blog> blogs = new List<Blog>();
        public List<BlogDTO> blogss = new List<BlogDTO>();

        public BlogController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            blogApiUrl = AppConstant.API_URL + "/blogs";
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(blogApiUrl + "/get-all");
            var data = await response.Content.ReadAsStringAsync();
            blogs = JsonSerializer.Deserialize<List<Blog>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            ViewBag.blogs = blogs;
            ViewBag.pageTitle = "Quản lý bài viết";
            ViewBag.JsFiles = new List<string> { "list.js" };
            return View();
        }
        public async Task<IActionResult> BlogDetails(int id)
        {
            HttpResponseMessage response = await client.GetAsync(blogApiUrl + "/get-blog/" + id);
            Blog blog = new Blog();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                blog = response.Content.ReadFromJsonAsync<Blog>().Result;
            }
            ViewBag.pageTitle = "Xem chi tiết Blog";
            return View(blog);
        }
        public async Task<IActionResult> Remove(int id)
        {
            await client.DeleteAsync(blogApiUrl + "/remove/" + id);
            //ViewBag.JsFiles = new List<string> { "global.js" };
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create(BlogDTO blogDTO)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonSerializer.Serialize(blogDTO), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(blogApiUrl + "/create", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tạo bài viết thất bại.");
                }
            }

            ViewBag.pageTitle = "Tạo Mới bài viết";
            ViewBag.JsFiles = new List<string> { "blog.js" };
            return View(blogDTO);
        }

        //public async Task<IActionResult> Create(BlogCreateDTO blogDTO)
        //{


        //    var content = new StringContent(JsonSerializer.Serialize(blogDTO), Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await client.PostAsync(blogApiUrl + "/create-blog-uploadfile", content);

        //    if (response.IsSuccessStatusCode)
        //    {

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Tạo bài viết thất bại.");
        //        return View(blogDTO);
        //    }


        //    ViewBag.pageTitle = "Tạo Mới bài viết";
        //    return View(blogDTO);

        //}

        public async Task<IActionResult> Update(int id)
        {
            HttpResponseMessage blogDetailResponse = await client.GetAsync(blogApiUrl + "/get-blog/" + id);
            BlogDTO blog = new BlogDTO();
            if (blogDetailResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                blog = blogDetailResponse.Content.ReadFromJsonAsync<BlogDTO>().Result;
            }
            ViewBag.pageTitle = "Chỉnh sửa bài viết";
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogDTO blogDTO)
        {         
            if (ModelState.IsValid)
            {
                try
                {
                                 
                    HttpResponseMessage blogResponse = await client.PutAsJsonAsync(blogApiUrl + "/edit/" + id, blogDTO);

                    if (blogResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Edit", new { id = id });
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error occurred while processing the request. " + ex.Message;
                }
            }
            ViewBag.pageTitle = "Chỉnh sửa bài viết";

            return View(blogDTO);
        }
    }
}
