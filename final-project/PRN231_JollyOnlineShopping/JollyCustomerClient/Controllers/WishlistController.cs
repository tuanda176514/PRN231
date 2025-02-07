using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JollyCustomerClient.Controllers
{
    public class WishlistController : Controller
    {
        private readonly JollyShoppingOnlineContext db;
        public WishlistController (JollyShoppingOnlineContext db)
        {
            this.db = db;
        }
		public IActionResult Index(int? uid)
        {
            if(uid.HasValue)
            {
                var user = db.Users.Find(uid);
                if (user != null)
                {
					List<int?> productIds = db.WishItems.Where( x=> x.WishlistId == uid )
                        .Select(x => x.ProductId).ToList();
                    ViewBag.Products = db.Products.Where(x => productIds.Contains(x.Id))
                        .Include(x => x.Images).ToList();
                    return View();
                }
            }
            return Redirect("/");
        }
    }
}
