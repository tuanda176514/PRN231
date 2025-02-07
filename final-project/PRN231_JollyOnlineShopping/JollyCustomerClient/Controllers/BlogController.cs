
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using JollyCustomerClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;


namespace JollyCustomerClient.Controllers
{
    public class BlogController : Controller
    {
        private readonly JollyShoppingOnlineContext db;
        private readonly HttpClient client;
        private string blogApiUrl;
        private string cmtApiUrl;
        public List<Blog> blogs = new List<Blog>();
        public List<Comment> comments = new List<Comment>();
        public CommentDTO cmts = new CommentDTO();
        // public List<BlogDTO> blogss = new List<BlogDTO>();

        public BlogController(JollyShoppingOnlineContext db)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            blogApiUrl = AppConstant.API_URL + "/blogs";
            cmtApiUrl = AppConstant.API_URL + "/comments";
            this.db = db;
        }
        public async Task<IActionResult> Index(string name = "", int page = 1)
        {
            if (string.IsNullOrEmpty(name)) name = "";
            int pageSize = 6;
            int skip = (page - 1) * pageSize;
            List<Blog> blogs; 

          
            HttpResponseMessage response = await client.GetAsync(blogApiUrl + "/get-all");
            var data = await response.Content.ReadAsStringAsync();
            blogs = JsonSerializer.Deserialize<List<Blog>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

         
            ViewBag.blogs = blogs.Skip(skip).Take(pageSize).ToList();
            ViewBag.name = name;
            ViewBag.page = page;
            var totalBlog = blogs.Count();
            ViewBag.totalBlog = totalBlog;
            ViewBag.totalPage = (int)Math.Ceiling((double)totalBlog / pageSize);

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
            // ViewBag.pageTitle = "Chi tiết Blog";
            return View(blog);


        }
        [Route("/blog/detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            ViewData["JsFiles"] = new List<string>() { "blog/comment.js" };

            HttpResponseMessage blogResponse = await client.GetAsync(blogApiUrl + "/get-blog/" + id);
            Blog blog = new Blog();
            if (blogResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                blog = blogResponse.Content.ReadFromJsonAsync<Blog>().Result;
            }

           
            HttpResponseMessage commentResponse = await client.GetAsync(cmtApiUrl + $"/get-comment/{id}");
            if (commentResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                comments = commentResponse.Content.ReadFromJsonAsync<List<Comment>>().Result;
            }


            ViewBag.Comments = comments; 

            return View(blog);
        }
        public IActionResult WriteComment(int id, CommentDTO commentDTO)
        {

            var userId = HttpContext.Session.GetInt32("userId");
            if (!userId.HasValue || userId.Value <= 0)
            {
                return Redirect("/User/Login");
            }

            var blog = db.Products.FirstOrDefault(p => p.Id == id);

            Comment newComment = new Comment
            {
                Content = commentDTO.Content,
                BlogId = id,
                UserId = userId,
                Date = DateTime.Now
            };

            db.Comments.Add(newComment);
            db.SaveChanges();

            return RedirectToAction("Detail", "Blog", new { id = blog.Id, success = true });
        }

    }
}
