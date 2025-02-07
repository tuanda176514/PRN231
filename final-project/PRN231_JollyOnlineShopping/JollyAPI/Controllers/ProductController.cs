using JollyAPI.DAO;
using JollyAPI.Helper;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.DTOS.Statistic;
using JollyAPI.Models.Entity;
using JollyAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JollyAPI.Controllers
{
    [Route("api/[products]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;
        private readonly RatingService ratingService;

        public ProductController(ProductService productService, RatingService ratingService)
        {
            this.productService = productService;
            this.ratingService = ratingService;
        }

        [HttpGet("/products/get-all")]
        public ActionResult<IList<ProductDTO>> GetAllProducts() => productService.GetAllProducts();

        [HttpGet("/products/get-all-color")]
        public ActionResult<IEnumerable<ColorDTO>> GetAllColors()
        {
            var colors = productService.GetColors();
            var colorDTOs = colors.Select(c => new ColorDTO { Id = c.Id, Name = c.Name, Hex = c.Hex}).ToList();
            return Ok(colorDTOs);
        }

        [HttpGet("/products/get-color/{id}")]
        public IActionResult GetColorsByProductId(int id)
        {
            try
            {
                var colors = productService.GetColorsByProductId(id);
                return Ok(colors);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("/products/get-image/{id}")]
        public IActionResult GetImagesByProductId(int id)
        {
            try
            {
                var images = productService.GetImagesByProductId(id);
                return Ok(images);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("/products/get-product/{id}")]
        public ActionResult<ProductDTO> GetProductById(int id) => productService.GetProductById(id);

        //[HttpPost("/products/create")]
        //public IActionResult AddProduct([FromForm] ProductCreateDTO product)
        //{
        //    try
        //    {
        //        productService.CreateProduct(product);

        //        return Ok(new { Message = "Product added successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
        //    }
        //}

        [Consumes("multipart/form-data")]
        [HttpPost("/products/create")]
        public IActionResult AddProduct([FromForm] ProductCreateDTO product, List<IFormFile> imageFiles)
        {
            try
            {
                foreach (var file in imageFiles)
                {
                    if (!IsImageFile(file))
                    {
                        return BadRequest(new { Message = "Invalid file type. Only image files are allowed." });
                    }
                }

                if (imageFiles.Count < 4)
                {
                    return BadRequest(new { Message = "Invalid number of files. You must upload at least 4 image files." });
                }

                productService.AddProduct(product, imageFiles);

                return Ok(new { Message = "Product added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
            }
        }

        private bool IsImageFile(IFormFile file)
        {
            var allowedImageTypes = new List<string> { "image/jpeg", "image/png", "image/gif" };

            return allowedImageTypes.Contains(file.ContentType);
        }

        [HttpPut("/products/edit/{id}")]
        public IActionResult EditProduct(int id, [FromForm] ProductUpdateDTO product, [FromForm] List<IFormFile> imageFiles)
        {
            try
            {
                foreach (var file in imageFiles)
                {
                    if (!IsImageFile(file))
                    {
                        return BadRequest(new { Message = "Invalid file type. Only image files are allowed." });
                    }
                }

                if (imageFiles.Count < 2)
                {
                    return BadRequest(new { Message = "Invalid number of files. You must upload at least 2 image files." });
                }
                productService.UpdateProduct(id, product, imageFiles);
                return Ok(new { Message = "Product edited successfully." });
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpDelete("/products/delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            productService.DeleteProduct(id);
            return Ok(new {Message = "Product deleted successfuly."});
        }

        [HttpDelete("/products/image/delete/{pid}/{uuid}")]
        public IActionResult DeleteImageByProductId(int pid, int uuid)
        {
            productService.DeleteImageByProductId(pid, uuid);
            return Ok("Delete Image Successfully.");
        }

        //[HttpDelete("/products/delete-image/{productId}")]
        //public IActionResult DeleteImage(int productId)
        //{
        //    try
        //    {
        //        JollyShoppingOnlineContext _context = new JollyShoppingOnlineContext();
        //        var product = _context.Products
        //    .Include(p => p.Images)
        //    .FirstOrDefault(p => p.Id == productId);

        //        if (product == null)
        //        {
        //            return NotFound("Product not found");
        //        }

        //        foreach (var image in product.Images.ToList())
        //        {
        //            Uri uri = new Uri(image.Url);

        //            string fileName = Path.GetFileName(uri.LocalPath);

        //            var imagePath = Path.Combine(AppConstantAPI.PATH, AppConstantAPI.IMAGEPRODUCTPATH, fileName);

        //            if (System.IO.File.Exists(imagePath))
        //            {
        //                System.IO.File.Delete(imagePath);
        //            }

        //            product.Images.Remove(image);
        //        }

        //        _context.SaveChanges();

        //        return Ok("All images for the product deleted successfully");
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
            
        //}


        [HttpGet("/products/{id}/reviews")]
        public IActionResult GetProductReviews(int id, [FromQuery] int? size = null, [FromQuery] int? page = null)
        {
            try
            {
                var reviews = ratingService.GetProductReviews(id);
                // Áp dụng phân trang nếu size và page được cung cấp
                if (size.HasValue && page.HasValue)
                {
                    reviews = reviews.Skip((page.Value - 1) * size.Value)
                                     .Take(size.Value)
                                     .ToList();
                }

                return Ok(reviews);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("/products/{id}/reviews")]
        public IActionResult WriteReview(int id, [FromBody] RatingDTO ratingDTO)
        {
            try
            {
                ratingService.WriteReview(id, ratingDTO);

                return Ok("Review added successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/products/statistics")]
        public IActionResult Statistics()
        {
            StatisticReport report = productService.getReport();
            return Ok(report);
        }
    }
}
