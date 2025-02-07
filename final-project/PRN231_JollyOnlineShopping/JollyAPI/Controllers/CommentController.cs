using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using JollyAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JollyAPI.Controllers
{
    
    [Route("api/[comments]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentService cmtService;
        public CommentController(CommentService cmtService)
        {
            this.cmtService = cmtService;
        }
        [HttpGet("/comments")]
        public ActionResult<IList<Comment>> GetComment() => cmtService.GetAllComments();


        [HttpGet("/comments/get-comment/{id}")]
        public ActionResult<IList<Comment>> GetListCommentByBlogId(int id) => cmtService.GetListCommentByBlogId(id);

        //[HttpPost("create-comment/{blogid}/{userid}/{content}")]
        //public ActionResult<CommentDTO> CreateComment(int blogid, int userid, string content)
        //{
        //    try
        //    {             
        //        var newComment = cmtService.CreateComment(blogid, userid, content);

        //        return Ok(newComment);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal Server Error: " + ex.Message);
        //    }
        //}




    }
}
