using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IProductRepository
    {
        void SaveProduct(Product Product);
        Product GetProductById(int id);
        List<Product> GetProducts();
        void UpdateProduct(Product Product);
        void DeleteProduct(Product Product);
    }
}
