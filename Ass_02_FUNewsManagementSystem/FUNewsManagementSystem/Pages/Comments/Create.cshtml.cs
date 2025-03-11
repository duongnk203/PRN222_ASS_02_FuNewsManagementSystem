using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FUNewsManagementSystem.Pages.Comments
{
    public class CreateModel : PageModel
    {
        private ICommentRepository _commentRepository;
        public CreateModel(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        [BindProperty]
        public string Content { get; set; }
        [BindProperty]
        public int NewsArticleId { get; set; }
        public IActionResult OnPost()
        {
            var message = "";
            var userId = User.FindFirst("AccountID")?.Value;
            if (userId == null)
            {
                return Unauthorized(); // Người dùng chưa đăng nhập
            }

            if (string.IsNullOrWhiteSpace(Content) || Content.Length > 1000)
            {
                ModelState.AddModelError("", "Nội dung bình luận không hợp lệ.");
                return Page();
            }
            var comment = new Comment
            {
                NewsArticleId = NewsArticleId,
                Content = Content.Trim(),
                AccountId = (short) Int32.Parse(userId),
                CreatedDate = DateTime.Now
            };
            _commentRepository.CreateComment(comment, out message);
            if (!string.IsNullOrWhiteSpace(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }
            return RedirectToPage("/NewsArticles/Details", new {id = NewsArticleId });
        }
    }
}
