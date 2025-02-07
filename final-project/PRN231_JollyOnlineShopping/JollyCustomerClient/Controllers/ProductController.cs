using JollyAPI.Models.Entity;
using JollyCustomerClient.Models;
using JollyCustomerClient.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Microsoft.AspNetCore.Cors;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.DTOS.User;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using System.Drawing.Printing;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace JollyCustomerClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly JollyShoppingOnlineContext db;
        private readonly HttpClient client;
        private string productApiUrl;
        private string categoryApiUrl;
        public ProductController(JollyShoppingOnlineContext db)
        {
            this.db = db;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            productApiUrl = AppConstant.API_URL + "/products";
            categoryApiUrl = AppConstant.API_URL + "/categories";
        }
        public IActionResult Search()
        {
            ViewData["JsFiles"] = new List<string>() { "product/search.js", "cart/add.js" };
            //get random 6 products
            ViewBag.products = db.Products.Include(x => x.Images).OrderBy(_ => Guid.NewGuid()).Take(6).ToList();
            return View();
        }

        public IActionResult Index(string name = "", int page = 1, string sort = "des")
        {
            ViewData["JsFiles"] = new List<string>() { "cart/add.js" };
            if (string.IsNullOrEmpty(name)) name = "";
            int pageSize = 16;
            int skip = (page - 1) * pageSize;
            IQueryable<Product> products;
            if (sort.Equals("asc"))
            {
                products = db.Products.OrderBy(x => x.Price)
                    .Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            else
            {
                products = db.Products.OrderByDescending(x => x.Price)
                    .Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            ViewBag.products = products.Skip(skip).Take(pageSize).Include(x => x.Images).ToList();
            ViewBag.name = name;
            ViewBag.sort = sort;
            ViewBag.page = page;
            var totalProduct = products.Count();
            ViewBag.totalProduct = totalProduct;
            ViewBag.totalPage = (int)Math.Ceiling((double)totalProduct / pageSize);
            return View();
        }

        public IActionResult Category(int id, string gender = "", int page = 1, string sort = "des")
        {
            ViewData["JsFiles"] = new List<string>() { "cart/add.js" };
            Category category = db.Categories.Find(id);
            IQueryable<Product> products = db.Products.Include(x => x.Category);
            if (sort.Equals("asc"))
            {
                products = products.OrderBy(x => x.Price);
            }
            else
            {
                products = products.OrderByDescending(x => x.Price);
            }

            if (category != null && category.ParentId != null)
            {
                products = products.Where(p => p.CategoryId == id);
            }
            else if (category != null)
            {
                products = products.Where(p => p.Category.ParentId == category.Id);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                products = products.Where(x => x.Gender.ToLower().Equals(gender.ToLower()));
            }

            int pageSize = 16;
            int skip = (page - 1) * pageSize;
            ViewBag.products = products.Skip(skip).Take(pageSize).Include(x => x.Images).ToList();
            ViewBag.id = id;
            string name = category != null ? category.Name : "";
            if (!string.IsNullOrEmpty(name)
                && !string.IsNullOrEmpty(FormatString.TranslateGender(gender)))
            {
                ViewBag.name = name + " - " + FormatString.TranslateGender(gender);
            }
            else
            {
                ViewBag.name = name + FormatString.TranslateGender(gender);
            }
            ViewBag.gender = gender;
            ViewBag.sort = sort;
            ViewBag.page = page;
            var totalProduct = products.Count();
            ViewBag.totalPage = (int)Math.Ceiling((double)totalProduct / pageSize);
            ViewBag.totalProduct = totalProduct;

            return View();
        }

        [Route("/product/detail/{id}")]
        public async Task<IActionResult> Detail(int id, int page = 1)
        {
            ViewData["JsFiles"] = new List<string>() { "cart/add.js", "product/rating.js" };

            HttpResponseMessage response = await client.GetAsync(productApiUrl + "/get-product/" + id);
            Product product = new Product();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                product = response.Content.ReadFromJsonAsync<Product>().Result;
            }
            HttpResponseMessage response2 = await client.GetAsync(productApiUrl + "/get-color/" + id);
            List<Color> color = new List<Color>();
            if (response2.StatusCode == System.Net.HttpStatusCode.OK)
            {
                color = response2.Content.ReadFromJsonAsync<List<Color>>().Result;
            }
            HttpResponseMessage response3 = await client.GetAsync(productApiUrl + "/" + id + "/reviews");

            int size = 6;

            var ratings = db.Ratings.Include(r => r.User)
                .Where(r => r.ProductId == id)
                .OrderByDescending(r => r.Date)
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            var reviews = await db.Ratings.Where(r => r.ProductId == id).CountAsync();
            var average = await db.Ratings.Where(r => r.ProductId == id && r.Quantity.HasValue)
                                                .AverageAsync(r => r.Quantity);
            ViewBag.PageNumber = page;
            ViewBag.PageSize = size;
            ViewBag.TotalRecords = db.Ratings.Include(r => r.User)
                                             .Where(r => r.ProductId == id)
                                             .OrderByDescending(r => r.Date).ToList().Count();
            ViewBag.reviews = reviews;
            ViewBag.average = average;
            ViewBag.ratings = ratings;
            ViewBag.colors = color;
            ViewBag.random1 = db.Products.OrderBy(_ => Guid.NewGuid()).Take(4).Include(x => x.Images).ToList();
            ViewBag.InWishlist = db.WishLists.Where(x => x.UserId == HttpContext.Session.GetInt32("userId"))
                                            .Include(x => x.WishItems)
                                            .FirstOrDefault()?.WishItems.Select(x => x.ProductId).Contains(id);
            
            return View(product);
        }

        public IActionResult WriteReview(int id, RatingDTO ratingDTO)
        {

            var userId = HttpContext.Session.GetInt32("userId");
            if (!userId.HasValue || userId.Value <= 0)
            {
                return Redirect("/User/Login");
            }

            var product = db.Products.FirstOrDefault(p => p.Id == id);

            if (ratingDTO.Quantity < 1 || ratingDTO.Quantity > 5)
            {
                throw new ArgumentException("Invalid quantity. Quantity should be between 1 and 5.");
            }

            Rating newReview = new Rating
            {
                Content = ratingDTO.Content,
                Quantity = ratingDTO.Quantity,
                ProductId = id,
                UserId = userId,
                Date = DateTime.Now
            };

            db.Ratings.Add(newReview);
            db.SaveChanges();
            return RedirectToAction("Detail", "Product", new { id = product.Id, success = true });

        }


    }
}
