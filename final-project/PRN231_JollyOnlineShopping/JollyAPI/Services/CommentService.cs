using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;

namespace JollyAPI.Services
{
    public class CommentService
    {
        private readonly CommentDAO commentDAO;
        public CommentService(CommentDAO commentDAO)
        {
            this.commentDAO = commentDAO;
        }

        public List<Comment> GetAllComments() => commentDAO.GetComments();

        public List<Comment> GetListCommentByBlogId(int blogId) => commentDAO.GetListCommentByBlogId(blogId);
        public CommentDTO CreateComment(int blogId, int userId, string content) => commentDAO.CreateComment(blogId,userId,content);
    }
}
