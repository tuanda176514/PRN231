
using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;

namespace JollyAPI.Services
{
    public class RatingService
    {
        private readonly RatingDAO ratingDAO;

        public RatingService(RatingDAO ratingDAO)
        {
            this.ratingDAO = ratingDAO;
        }

        public List<Rating> GetProductReviews(int? id) => ratingDAO.GetProductReviews(id);
        public void WriteReview(int id, RatingDTO ratingDTO) => ratingDAO.WriteReview(id, ratingDTO);
    }
}
