using AutoMapper;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JollyAPI.DAO
{
    public class RatingDAO
    {
        private readonly JollyShoppingOnlineContext _context;
        private readonly IMapper _mapper;

        public RatingDAO(JollyShoppingOnlineContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Rating> GetProductReviews(int? productId)
        {
            return _context.Ratings
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.Date)
                .ToList();

            //int size = 6;
            //int pageNumber = (page ?? 1);
            //return _context.Ratings
            //    .Include(r => r.User)
            //    .Where(r => r.ProductId == productId)
            //    .OrderByDescending(r => r.Date)
            //    .Skip((pageNumber - 1) * size)
            //                    .Take(size)
            //    .ToList();

        }

        public void WriteReview(int productId, RatingDTO ratingDTO)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            if (ratingDTO.Quantity < 1 || ratingDTO.Quantity > 5)
            {
                throw new ArgumentException("Invalid quantity. Quantity should be between 1 and 5.");
            }

            var newReview = new Rating
            {
                Content = ratingDTO.Content,
                Quantity = ratingDTO.Quantity,
                Date = DateTime.Now,
                ProductId = productId,
                UserId = ratingDTO.UserId
            };

            _context.Ratings.Add(newReview);
            _context.SaveChanges();
        }

    }
}
