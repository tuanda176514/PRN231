using JollyAPI.Models.Entity;

namespace JollyAPI.Models.DTOS
{
    public class RatingDTO
    {
        public string Content { get; set; }
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Date { get; set; }

    }
}
