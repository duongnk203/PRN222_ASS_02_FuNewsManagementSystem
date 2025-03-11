using FUNewsManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Comments
{
    public class DeleteModel : PageModel
    {
        private readonly ICommentRepository _commentRepository;
        public DeleteModel(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        [BindProperty]
        public int NewsArticleId { get; set; }
        [BindProperty]
        public int CommentID { get; set; }
        public IActionResult OnPost()
        {
            var message = "";
            _commentRepository.DeleteComment(CommentID, out message);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }
            return RedirectToPage("/NewsArticles/Details", new {id = NewsArticleId});
        }
    }
}
