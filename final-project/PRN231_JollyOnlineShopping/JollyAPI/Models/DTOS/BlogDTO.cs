namespace JollyAPI.Models.DTOS
{
	public class BlogDTO
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string ShortContent { get; set; }
		public string Content { get; set; }
		public DateTime? PublishedDate { get; set; }
		public string? Image { get; set; }
		public int? UserId { get; set; }

        //public IFormFile ImageFiles { get; set; }
    }
}
