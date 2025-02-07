using JollyAPI.Models.Entity;

namespace JollyAPI.DAO
{
    public class CategoryDAO
    {
        private readonly JollyShoppingOnlineContext _context;

        public CategoryDAO(JollyShoppingOnlineContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.Where(c => c.ParentId != null).ToList();
        }
    }
}
