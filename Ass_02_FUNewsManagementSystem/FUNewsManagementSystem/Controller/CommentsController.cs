using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;
using FUNewsManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly SignalRServer _signalRServer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommentsController(ICommentRepository commentRepository, SignalRServer signalRServer, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _signalRServer = signalRServer;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetComments")]
        public IActionResult GetComments(int newsArticleId)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            int role = int.TryParse(user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value, out int parsedRole) ? parsedRole : 0;
            int accountId = int.TryParse(user?.Claims.FirstOrDefault(c => c.Type == "AccountID")?.Value, out int parsedAccountId) ? parsedAccountId : -1;

            var message = "";
            var comments = _commentRepository.GetComments(newsArticleId, role, accountId, out message);
            //if (!string.IsNullOrEmpty(message))
            //{
            //    return BadRequest(new { error = message });
            //}

            return Ok(comments);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentViewModel createComment)
        {
            if (string.IsNullOrWhiteSpace(createComment.Content))
            {
                return BadRequest("Noi dung binh luan khong duoc de trong");
            }
            var accountIdClaim = User.FindFirst("AccountID")?.Value;
            if (string.IsNullOrEmpty(accountIdClaim))
            {
                return RedirectToPage("/Authentication/AccessDenied");
            }
            int accountID = int.Parse(accountIdClaim);

            var message = "";
            var comment = new Comment()
            {
                Content = createComment.Content,
                CreatedDate = DateTime.Now,
                IsActive = true,
                NewsArticleId = createComment.NewsArticleId,
                AccountId = (short)accountID
            };
            _commentRepository.CreateComment(comment, out message);
            await _signalRServer.Clients.All.SendAsync("ReceiveCommentUpdate");
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest($"Could not create comment {message}");
            }
            return Ok(comment);
        }

        [HttpPost("UpdateComment")]
        public IActionResult UpdateComment([FromBody]UpdateComment updateComment)
        {
            var message = "";
            _commentRepository.Update(updateComment.CommentId, updateComment.Content, out message);
            _signalRServer.Clients.All.SendAsync("ReceiveCommentUpdate");
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            return Ok(updateComment);
        }

        [HttpDelete("Delete/{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            var message = "";
            _commentRepository.DeleteComment(commentId, out message);
            _signalRServer.Clients.All.SendAsync("ReceiveCommentUpdate");
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }
            return Ok();
        }
    }
}
