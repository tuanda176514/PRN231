using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;


namespace JollyAPI.Services
{
    public class BlogService
    {
		private readonly BlogDAO blogDAO;
		public BlogService(BlogDAO blogDAO)
		{
			this.blogDAO = blogDAO;
		}

		public List<BlogDTO> GetAllBlogs() => blogDAO.GetBlog();
		public void AddBlog(BlogCreateDTO blogDTO,IFormFile imageFiles) => blogDAO.PostBlog(blogDTO, imageFiles);
		public void CreateBlog(BlogDTO blogDTO) => blogDAO.CreateBlog(blogDTO);
        public void UpdateBlog(int id, BlogDTO blogDTO) => blogDAO.UpdateBlogById(id, blogDTO);
        public void EditBlog(int id, BlogDTO blogDTO) => blogDAO.EditBlog(id, blogDTO);
        public void DeleteBlog(int id) => blogDAO.DeleteBlog(id);
        public void RemoveBlog(int id) => blogDAO.RemoveBlog(id);
        public BlogDTO GetBlogById(int blogId) => blogDAO.GetBlogById(blogId);
    }
}
