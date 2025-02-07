using JollyAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JollyAPI.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class WishListController : ControllerBase
	{
		public readonly WishListService service;
		public WishListController(WishListService service)
		{
			this.service = service;
		}

		[HttpGet]
		public IActionResult TotalItem(int uid)
		{
			return Ok(service.TotalItem(uid));
		}

		[HttpPost]
		public IActionResult ModifyItem(int uid,int pid)
		{
			try
			{
				return Ok(service.ModifyItem(uid, pid));
			}catch (Exception) {
				return BadRequest("Product doesn't exist or already in wishlist");
			}
		}
	}
}
