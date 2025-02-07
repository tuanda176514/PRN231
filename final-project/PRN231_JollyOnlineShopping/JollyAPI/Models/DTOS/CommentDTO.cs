namespace JollyAPI.Models.DTOS
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? UserId { get; set; }
        public int? BlogId { get; set; }
        public DateTime? Date { get; set; }
        public string FullName { get; set; } // Add the FullName property
    }
}
