using JollyAPI.Models.Entity;

namespace JollyAPI.Models.DTOS
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Gender { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ImageDTO> Images { get; set; }
        public List<SizeDTO> Sizes { get; set; }

        public ProductDTO()
        {
            Images = new List<ImageDTO>();
            Sizes = new List<SizeDTO>();
        }

    }
}
