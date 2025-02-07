using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using JollyAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JollyAPI.Controllers
{
    [Route("api/[categories]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService categoryService;

        public CategoryController(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("/categories/get-all")]
        public ActionResult<IList<Category>> GetAllCategories() => categoryService.GetCategories();
    }
}
