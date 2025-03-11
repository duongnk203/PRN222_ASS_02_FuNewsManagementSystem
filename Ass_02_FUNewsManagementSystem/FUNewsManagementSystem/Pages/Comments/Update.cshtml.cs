using FUNewsManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Comments
{
    public class UpdateModel : PageModel
    {
        private readonly ICommentRepository _commentRepository;
        public UpdateModel(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [BindProperty]
        public int CommentId { get; set; }
        [BindProperty]
        public int NewsArticleId { get; set; }
        [BindProperty]
        public string UpdatedContent { get; set; }
        public IActionResult OnPost()
        {
            var message = "";
            _commentRepository.Update(CommentId, UpdatedContent, out message);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }
            return RedirectToPage("/NewsArticles/Details", new { id = NewsArticleId });
        }
    }
}
