using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using (var context = new MyDBContext())
                {
                    listProducts = context.Products.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listProducts;
        }

        public static Product FindProductById(int productId)
        {
            var product = new Product();
            try
            {
                using (var context = new MyDBContext())
                {
                    product = context.Products.SingleOrDefault(c => c.ProductId == productId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public static void SaveProduct(Product product)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    context.Products.Add(product);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateProduct(Product product)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    context.Entry(product).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteProduct(Product product)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var productToDelete = context
                        .Products
                        .SingleOrDefault(c => c.ProductId == product.ProductId);
                    context.Products.Remove(productToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
