using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JollyAPI.DAO
{
    public class CommentDAO
    {
        private readonly JollyShoppingOnlineContext _context;

        public CommentDAO(JollyShoppingOnlineContext context)
        {
            _context = context;
        }

        public List<Comment> GetComments()
        {
            var cmt = new List<Comment>();
            try
            {
                using (var context = new JollyShoppingOnlineContext())
                {
                    cmt = context.Comments.ToList();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return cmt;
        }
        public List<Comment> GetListCommentByBlogId(int? blogId)
        {
           return _context.Comments
                 .Include(r => r.User)
                 .Where(r => r.BlogId == blogId)
                 .OrderByDescending(r => r.Date)
                 .ToList();
        }
        public CommentDTO CreateComment(int blogId, int userId, string content)
        {
            try
            {
                var comment = new Comment
                {
                    BlogId = blogId,
                    UserId = userId,
                    Content = content,
                    Date = DateTime.Now 
                };

                _context.Comments.Add(comment);
                _context.SaveChanges();

                // Return the newly created comment as a DTO
                return new CommentDTO
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    Date = comment.Date,
                    BlogId = comment.BlogId,
                    UserId = comment.UserId
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
