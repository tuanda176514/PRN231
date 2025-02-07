using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.DTOS.Statistic;
using JollyAPI.Models.Entity;

namespace JollyAPI.Services
{
    public class ProductService
    {
        private readonly ProductDAO productDAO;
        private readonly OrderDAO orderDAO;
		public ProductService(ProductDAO productDAO, OrderDAO orderDAO)
        {
            this.productDAO = productDAO;
            this.orderDAO = orderDAO;
        }

        public List<ProductDTO> GetAllProducts() => productDAO.GetProducts();
        public List<ColorDTO> GetColorsByProductId(int id) => productDAO.GetColorsByProductId(id);
        public List<Image> GetImagesByProductId(int id) => productDAO.GetImagesByProductId(id);
        public void DeleteImageByProductId(int productId, int imageId) => productDAO.DeleteImageByProductId(productId, imageId);
        public List<Color> GetColors() => productDAO.GetColors();
        public ProductDTO GetProductById(int productId) => productDAO.GetProductById(productId);
        public void AddProduct(ProductCreateDTO product, List<IFormFile> imageFiles) => productDAO.AddProduct(product, imageFiles);
        public void UpdateProduct(int id, ProductUpdateDTO product, List<IFormFile> imageFiles) => productDAO.UpdateProduct(id, product, imageFiles);
        public void DeleteProduct(int id) => productDAO.DeleteProduct(id);

        public StatisticReport getReport()
        {
            StatisticReport report = new StatisticReport();
            report.TopProductReport = productDAO.GetTopProductSale();  
            report.RecentOrders = orderDAO.getRecentOrder();
            productDAO.GetMonthTotal(report);
            return report;
        }
    }
}
