using BusinessObjects;
using DataAccess;

namespace Repositories.impl
{
    public class ProductRepository : IProductRepository
    {
        public void SaveProduct(Product Product) => ProductDAO.SaveProduct(Product);
        public Product GetProductById(int id) => ProductDAO.FindProductById(id);
        public List<Product> GetProducts() => ProductDAO.GetProducts();
        public void UpdateProduct(Product Product) => ProductDAO.UpdateProduct(Product);
        public void DeleteProduct(Product Product) => ProductDAO.DeleteProduct(Product);
    }
}
