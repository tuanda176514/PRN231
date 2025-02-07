using System.Diagnostics;
using JollyAdminClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace JollyAdminClient.Controllers
{
	public class TestController : Controller
	{
		private readonly ILogger<TestController> _logger;

		public TestController(ILogger<TestController> logger)
		{
			_logger = logger;
		}

	
		public IActionResult EmptyLayout()
		{
			ViewData["JsFiles"] = new List<string>() { "test.js" };
			return View();
		}

		public IActionResult DetailExample()
		{
			ViewData["pageTitle"] = "Ví dụ 1 trang details bất kỳ";
			return View();
		}
	}
}