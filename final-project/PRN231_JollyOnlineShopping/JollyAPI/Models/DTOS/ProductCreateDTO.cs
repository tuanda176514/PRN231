using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JollyAPI.Models.DTOS
{
    public class ProductCreateDTO
    {

        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be a non-negative value")]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value")]
        public int? Quantity { get; set; }

        public int? CategoryId { get; set; }

        public int? ColorId { get; set; }

        public string Gender { get; set; }

        public IFormFileCollection ImageFiles { get; set; }
    }
}
