using System.Diagnostics;
using JollyAPI.Models.DTOS.Statistic;
using JollyAPI.Models.Entity;
using JollyCustomerClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JollyCustomerClient.Controllers
{
	public class HomeController : Controller
	{
		private readonly JollyShoppingOnlineContext db;

		public HomeController(JollyShoppingOnlineContext db)
		{
			this.db = db;
		}

		public IActionResult Index()
		{
			ViewData["JsFiles"] = new List<string>() { "cart/add.js", "user/login.js" };
			//ViewData["JsFiles"] = new List<string>() {"demo.js" };
			//ViewData["CssFiles"] = new List<string>() { "demo.css" };

			List<ProductReport> listProducts = db.OrderDetails.GroupBy(pr => pr.ProductId)
            .Select(g => new ProductReport
            {
                Id = g.Key,
                Quantity = (int)g.Sum(pr => pr.Quantity)
            }).OrderByDescending(x => x.Quantity).Take(8).ToList();
			List<Product> products = new List<Product>();
			foreach (var p in listProducts)
			{
				Product product = db.Products.Where(x => x.Id == p.Id).Include(x => x.Images).FirstOrDefault();
				products.Add(product);
			}

            // top 8 best seller
            ViewBag.products = products;
            //top 3 blogs
            ViewBag.blogs = db.Blogs.OrderByDescending(x => x.PublishedDate).Take(3).ToList();
			return View();
		}
	}
}