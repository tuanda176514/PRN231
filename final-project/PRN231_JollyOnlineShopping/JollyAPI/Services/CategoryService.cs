using JollyAPI.DAO;
using JollyAPI.Models.Entity;

namespace JollyAPI.Services
{
    public class CategoryService
    {
        private readonly CategoryDAO categoryDAO;

        public CategoryService(CategoryDAO categoryDAO)
        {
            this.categoryDAO = categoryDAO;
        }

        public List<Category> GetCategories() => categoryDAO.GetCategories();
    }
}
