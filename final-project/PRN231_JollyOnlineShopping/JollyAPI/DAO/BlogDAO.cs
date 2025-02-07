using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using JollyAPI.Helper;
using JollyAPI.Models.DTOS.User;

namespace JollyAPI.DAO
{
    public class BlogDAO
    {
        private readonly JollyShoppingOnlineContext _context;
        private readonly IMapper _mapper;
        public BlogDAO(JollyShoppingOnlineContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void CreateBlog(BlogDTO newBlogDTO)
        {          
            var newBlog = _mapper.Map<Blog>(newBlogDTO);         
            _context.Blogs.Add(newBlog);
            _context.SaveChanges();
        }
        private string SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                // Handle the case when no image file is provided or the file is empty.
                return null;
            }

            if (!IsImage(imageFile))
            {
                // Handle the case when the file is not an image.
                return null;
            }

            // Generate a unique file name for the uploaded image.
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);

            var uploadsFolder = Path.Combine(AppConstantAPI.PATH, AppConstantAPI.IMAGEBLOGPATH);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                // Ensure the target directory exists.
                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                return "https://localhost:8888/" + AppConstantAPI.IMAGEBLOGPATH + "/" + uniqueFileName;
            }
            catch (Exception ex)
            {
                // Handle the case when there is an error during image upload.
                Console.WriteLine("Lỗi tải lên ảnh: " + ex.Message);
                return null;
            }
        }

        private bool IsImage(IFormFile file)
        {
            if (file == null)
            {
                return false;
            }

            // Define a list of allowed image file extensions.
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension);
        }

        public void PostBlog(BlogCreateDTO blogDTO, IFormFile imageFile)
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == blogDTO.UserId);

            var imageUrl = SaveImage(imageFile);

            if (imageUrl != null)
            {
                var newBlog = _mapper.Map<Blog>(blogDTO);
                newBlog.Image = imageUrl;
                _context.Blogs.Add(newBlog);
                _context.SaveChanges();
            }
            else
            {
                // Handle the case when uploading the image fails.
            }
        }

        public void DeleteBlog(int blogId)
        {
            var blogToDelete = _context.Blogs
                .FirstOrDefault(b => b.Id == blogId);

            if (blogToDelete != null)
            {
                _context.Blogs.Remove(blogToDelete);
                _context.SaveChanges();
            }
        }

        public void RemoveBlog(int blogId)
        {
            var blogToDelete = _context.Blogs
                .Include(b => b.Comments)  // Load các comment liên quan
                .FirstOrDefault(b => b.Id == blogId);
            if (blogToDelete != null)
            {
                // Xóa tất cả các comment liên quan
                _context.Comments.RemoveRange(blogToDelete.Comments);
                // Xóa blog
                _context.Blogs.Remove(blogToDelete);
                _context.SaveChanges();
            }
        }

        public List<BlogDTO> GetBlog()
        {
            return _context.Blogs.Include(u => u.User).Select(b => new BlogDTO
            {
                Id = b.Id,
                Title = b.Title,
                ShortContent = b.ShortContent,
                Content = b.Content,
                PublishedDate = b.PublishedDate,
                Image = b.Image,
                UserId = b.UserId
            })
                .ToList();
        }

        public BlogDTO GetBlogById(int blogId)
        {
            return _context.Blogs
                .Where(b => b.Id == blogId)
                .Select(b => new BlogDTO
                {
                    Id = b.Id,
                    Content = b.Content,
                    Image = b.Image,
                    PublishedDate = b.PublishedDate,
                    ShortContent = b.ShortContent,
                    Title = b.Title,
                    UserId = b.UserId,
                })
                .FirstOrDefault();
        }

        public void UpdateBlogById(int blogID, BlogDTO blogDTO)
        {
            if (blogID == blogDTO.Id)
            {
                var updateBlog = _mapper.Map<Blog>(blogDTO);
                _context.Blogs!.Update(updateBlog);
                _context.SaveChanges();
            }
        }
        public BlogDTO EditBlog(int blogID, BlogDTO editedBlogDTO)
        {
            var existingBlog = _context.Blogs.FirstOrDefault(b => b.Id == blogID);
            if (existingBlog != null)
            {
              
                existingBlog.Title = editedBlogDTO.Title;
                existingBlog.ShortContent = editedBlogDTO.ShortContent;
                existingBlog.Content = editedBlogDTO.Content;
                existingBlog.PublishedDate = editedBlogDTO.PublishedDate;
                existingBlog.Image = editedBlogDTO.Image;
                existingBlog.UserId = 1;

                _context.SaveChanges();
                return _mapper.Map<BlogDTO>(existingBlog);
            }

            return null; 
        }
    }

}
