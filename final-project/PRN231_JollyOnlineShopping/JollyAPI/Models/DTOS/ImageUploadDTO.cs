using System.ComponentModel.DataAnnotations;

namespace JollyAPI.Models.DTOS
{
    public class ImageUploadDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int? ProductId { get; set; }
        public int? ColorId { get; set; }
        public ColorDTO Color { get; set; }
        public IFormFileCollection ImageFiles { get; set; }

    }
}
