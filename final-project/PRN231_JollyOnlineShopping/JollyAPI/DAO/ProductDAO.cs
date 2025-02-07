using AutoMapper;
using JollyAPI.Helper;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.DTOS.Statistic;
using JollyAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace JollyAPI.DAO
{
    public class ProductDAO
    {
        private readonly JollyShoppingOnlineContext _context;
        private readonly IMapper _mapper;

        public ProductDAO(JollyShoppingOnlineContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ProductDTO> GetProducts()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Sizes)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Gender = p.Gender,
                    CategoryId = p.CategoryId,
                    Category = p.Category,
                    Images = p.Images.Take(1).Select(i => new ImageDTO
                    {
                        Url = i.Url,
                    }).ToList(),
                    Sizes = p.Sizes.Select(s => new SizeDTO
                    {
                        Name = s.Name,
                    }).ToList()
                })
                .ToList();
        }

        public List<Color> GetColors()
        {
            return _context.Colors.ToList();
        }


        public List<ColorDTO> GetColorsByProductId(int productId)
        {
            return _context.Images
                .Where(i => i.ProductId == productId)
                .Select(i => new ColorDTO
                {
                    Id = i.Color.Id,
                    Name = i.Color.Name,
                    Hex = i.Color.Hex
                })
                .Distinct()
                .ToList();
        }

        public ProductDTO GetProductById(int productId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Sizes)
                .Where(p => p.Id == productId)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Gender = p.Gender,
                    CategoryId = p.CategoryId,
                    Category = p.Category,
                    Images = p.Images.Select(i => new ImageDTO
                    {
                        Url = i.Url,
                    }).ToList(),
                    Sizes = p.Sizes.Select(s => new SizeDTO
                    {
                        Name = s.Name,
                    }).ToList()
                })
                .FirstOrDefault();
        }

        public void AddProduct(ProductCreateDTO product, List<IFormFile> imageFiles)
        {
            var newProduct = _mapper.Map<Product>(product);


            _context.Products.Add(newProduct);
            _context.SaveChanges();

            foreach (var imageFile in imageFiles)
            {
                if (imageFile.Length > 0)
                {
                    var imageUrl = SaveImage(imageFile);

                    var newImageDTO = new ImageUploadDTO
                    {
                        Url = imageUrl,
                        ProductId = newProduct.Id,
                        ColorId = product.ColorId
                    };

                    var newImage = _mapper.Map<Image>(newImageDTO);
                    _context.Images.Add(newImage);
                }
            }

            _context.SaveChanges();

            var sizesToAdd = _context.Sizes.Take(4).ToList();
            foreach (var size in sizesToAdd)
            {
                newProduct.Sizes.Add(size);
            }

            _context.SaveChanges();
        }

        private string SaveImage(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), AppConstantAPI.PATH, AppConstantAPI.IMAGEPRODUCTPATH);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return "https://localhost:8888/" + AppConstantAPI.IMAGEPRODUCTPATH + "/" + uniqueFileName;

        }


        public void UpdateProduct(int id, ProductUpdateDTO product, List<IFormFile> imageFiles)
        {
            var existingProduct = _context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == id);

            if (existingProduct != null)
            {
                _mapper.Map(product, existingProduct);

                _context.SaveChanges();

                foreach (var imageFile in imageFiles)
                {
                    if (imageFile.Length > 0)
                    {
                        var imageUrl = SaveImage(imageFile);

                        var newImageDTO = new ImageUploadDTO
                        {
                            Url = imageUrl,
                            ProductId = existingProduct.Id,
                            ColorId = product.ColorId
                        };

                        var newImage = _mapper.Map<Image>(newImageDTO);
                        existingProduct.Images.Add(newImage);
                    }
                }

                _context.SaveChanges();
            }
        }


        public void DeleteProduct(int productId)
        {
            var product = _context.Products
                .Include(p => p.Images)
                .Include(p => p.Ratings)
                .Include(p => p.Sizes)
                .Include(p => p.Items)
                .Include(p => p.OrderDetails)
                .ThenInclude(od => od.Order)
                .Include(p => p.WishItems)
                .ThenInclude(wl => wl.Wishlist)
                .FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                foreach (var image in product.Images)
                {
                    Uri uri = new Uri(image.Url);

                    string fileName = Path.GetFileName(uri.LocalPath);

                    var imagePath = Path.Combine(AppConstantAPI.PATH, AppConstantAPI.IMAGEPRODUCTPATH, fileName);

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                foreach (var size in product.Sizes.ToList())
                {
                    product.Sizes.Remove(size);
                }

                _context.Items.RemoveRange(product.Items);


                foreach (var orderDetail in product.OrderDetails.ToList())
                {
                    var order = orderDetail.Order;

                    _context.OrderDetails.Remove(orderDetail);

                    if (!order.OrderDetails.Any())
                    {
                        _context.Orders.Remove(order);
                    }
                }

                foreach (var wishItem in product.WishItems.ToList())
                {
                    var wishList = wishItem.Wishlist;
                    _context.WishItems.Remove(wishItem);

                    if (!wishList.WishItems.Any())
                    {
                        _context.WishLists.Remove(wishList);
                    }
                }

                _context.Images.RemoveRange(product.Images);

                _context.Ratings.RemoveRange(product.Ratings);

                _context.Products.Remove(product);

                _context.SaveChanges();
            }
        }



        public void DeleteImageByProductId(int productId, int imageId)
        {
            var image = _context.Images.FirstOrDefault(x => x.ProductId == productId && x.Id == imageId);

            if (image != null)
            {
                Uri uri = new Uri(image.Url);

                string fileName = Path.GetFileName(uri.LocalPath);

                var imagePath = Path.Combine(AppConstantAPI.PATH, AppConstantAPI.IMAGEPRODUCTPATH, fileName);

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }

                _context.Images.Remove(image);
                _context.SaveChanges();
            }
        }



        public List<Image> GetImagesByProductId(int productId)
        {
            var images =  _context.Images
                        .Where(image => image.ProductId == productId)
                        .Select(image => new Image
                        {
                            Id = image.Id,
                            Url = image.Url
                        })
                        .ToList();
            
            return images;
        }

        public List<ProductReport> GetTopProductSale()
        {
            List<ProductReport> listProducts = _context.OrderDetails.GroupBy(pr => pr.ProductId)
            .Select(g => new ProductReport
            {
                Id = g.Key,
                Quantity = (int)g.Sum(pr => pr.Quantity)
            }).OrderByDescending(x => x.Quantity).Take(5).ToList();
            foreach (var p in listProducts)
            {
                p.Name = _context.Products.Where(x => x.Id == p.Id).Select(x => x.Name).FirstOrDefault();
                p.Url = _context.Images.Where(x => x.ProductId == p.Id).Select(x => x.Url).FirstOrDefault();
            }

            return listProducts;
        }

        public void GetMonthTotal(StatisticReport report)
        {
            try
            {
                var date = DateTime.Now;
                var currentMonth = new DateTime(date.Year, date.Month, 1);
                var nextMonth = currentMonth.AddMonths(1);
                report.TotalProducts = (int)_context.OrderDetails.Include(o => o.Order)
                    .Where(o => o.Order.Date >= currentMonth && o.Order.Date < nextMonth)
                    .Sum(x => x.Quantity);
                report.TotalOrders = _context.Orders
                    .Count();
                report.TotalMonthOrders = _context.Orders
                    .Where(o => o.Date >= currentMonth && o.Date < nextMonth)
                    .Count();
                report.TotalRevenue = (double)_context.OrderDetails.Include(o => o.Order)
                    .Where(o => o.Order.Date >= currentMonth && o.Order.Date < nextMonth)
                    .Sum(x => x.Quantity * x.Price);
                report.TotalRating = (float)_context.Ratings
                    .Where(o => o.Date >= currentMonth && o.Date < nextMonth)
                    .Average(x => x.Quantity);
            }
            catch (Exception e)
            {
            }
        }
    }
}
