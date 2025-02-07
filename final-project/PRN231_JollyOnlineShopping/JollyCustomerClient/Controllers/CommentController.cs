using Microsoft.AspNetCore.Mvc;

namespace JollyCustomerClient.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
