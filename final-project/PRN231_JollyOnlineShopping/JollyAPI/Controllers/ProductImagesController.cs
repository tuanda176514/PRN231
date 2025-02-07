using AutoMapper;
using JollyAPI.Helper;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JollyAPI.Controllers
{
    [Route("api/[images]/[action]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly JollyShoppingOnlineContext _context;
        private readonly IMapper _mapper;
        public ProductImagesController(JollyShoppingOnlineContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("/images/products/{productId}")]
        public async Task<IActionResult> UploadImages(int productId, [FromForm] List<IFormFile> imageFiles, [FromForm] int colorId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);

                if (product == null)
                {
                    return NotFound($"Product with ID {productId} not found");
                }

                var color = await _context.Colors.FindAsync(colorId);

                if (color == null)
                {
                    return NotFound($"Color with ID {colorId} not found");
                }

                foreach (var imageFile in imageFiles)
                {
                    if (imageFile.Length > 0)
                    {
                        var imageUrl = await SaveImageAsync(imageFile);

                        var newImageDTO = new ImageUploadDTO
                        {
                            Url = imageUrl,
                            ProductId = productId,
                            ColorId = colorId
                        };

                        var newImage = _mapper.Map<Image>(newImageDTO);

                        product.Images.Add(newImage);

                        await _context.SaveChangesAsync();
                    }
                }

                return Ok("Images uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), AppConstantAPI.PATH, AppConstantAPI.IMAGEPRODUCTPATH);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "https://localhost:8888/" + AppConstantAPI.IMAGEPRODUCTPATH + "/" + uniqueFileName;
        }

    }
}
