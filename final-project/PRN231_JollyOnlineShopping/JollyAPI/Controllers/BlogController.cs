
using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using JollyAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JollyAPI.Controllers
{
    [Route("api/[blogs]/[action]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogService blogService;
        private readonly JollyShoppingOnlineContext _context;
        public BlogController(JollyShoppingOnlineContext _context, BlogService blogService)
        {
            this.blogService = blogService;
            this._context = _context;
        }

        [HttpGet("/blogs/get-all")]
        public ActionResult<IList<BlogDTO>> GetAllBlogs() => blogService.GetAllBlogs();
        [HttpPost("/blogs/create")]
        public IActionResult CreateBlog([FromForm] BlogDTO newBlogDTO)
        {
            if (newBlogDTO.UserId == null)
            {
                newBlogDTO.UserId = 1;
            }
            try
            {
                blogService.CreateBlog(newBlogDTO);
                return CreatedAtAction("GetBlogById", new { id = newBlogDTO.Id }, new { Message = "Blog created successfully." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed to create blog.", Error = e.Message });
            }
        }
        [HttpPost("/blogs/create-blog-uploadfile")]
        public async Task<IActionResult> PostWithImage([FromForm] BlogCreateDTO blogDTO)
        {
            var blog = new Blog
            {
                Title = blogDTO.Title,
                ShortContent = blogDTO.ShortContent,
                Content = blogDTO.Content,
                PublishedDate = DateTime.Now,
                UserId = 1,

            };

            if (blogDTO.ImageFiles.Length > 0)
            {

                //xử lí ảnh 
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagesBlog");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + blogDTO.ImageFiles.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    blogDTO.ImageFiles.CopyToAsync(stream);
                }
                blog.Image = "https://localhost:8888/imagesBlog/" + uniqueFileName;
            }
            else
            {
                blog.Image = " ";
            }
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return Ok(blog);

        }
        [HttpGet("/blogs/get-blog/{id}")]
        public ActionResult<BlogDTO> GetBlogById(int id) => blogService.GetBlogById(id);
        [HttpDelete("/blogs/delete/{id}")]
        public IActionResult DeleteBlog(int id)
        {
            blogService.DeleteBlog(id);
            return Ok();
        }
        [HttpDelete("/blogs/remove/{id}")]
        public IActionResult RemoveBlog(int id)
        {
            blogService.RemoveBlog(id);
            return Ok();
        }
        [HttpPut("/blogs/update/{id}")]
        public IActionResult UpdateBlog(int id, [FromForm] BlogDTO blogDTO)
        {
            try
            {
                blogService.UpdateBlog(id, blogDTO);
                return Ok(new { Message = "Blog edited successfully.." });
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        [HttpPut("/blogs/edit/{id}")]
        public IActionResult EditBlog(int id, BlogDTO blogDTO)
        {
            try
            {
                blogService.EditBlog(id, blogDTO);
                return Ok(new { Message = "Blog edited successfully.." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed to edit blog.", Error = e.Message });
            }
        }
    }
}
