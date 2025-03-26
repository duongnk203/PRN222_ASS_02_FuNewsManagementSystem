using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;
using FUNewsManagementSystem.Pages.SystemAccounts;
using FUNewsManagementSystem.ViewModel;
using System.Security.Claims;

namespace FUNewsManagementSystem.Pages_NewsAriticles
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticalRepository _newsArticleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly SignalRServer _signalRServer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetailsModel(INewsArticalRepository newsArticalRepository, ICommentRepository commentRepository, SignalRServer signalRServer, IHttpContextAccessor httpContextAccessor)
        {
            _newsArticleRepository = newsArticalRepository;
            _commentRepository = commentRepository;
            _signalRServer = signalRServer;
            _httpContextAccessor = httpContextAccessor;
        }

        public NewsArticle NewsArticle { get; set; } = default!;
        public List<CommentDisplay> Comments { get; set; } = new List<CommentDisplay>();
        public AccountComment? accountComment { get; set; }

        [BindProperty]
        public int NewsArticleId { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _httpContextAccessor.HttpContext?.User;

            int role = int.TryParse(user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value, out int parsedRole) ? parsedRole : 0;
            int accountId = int.TryParse(user?.Claims.FirstOrDefault(c => c.Type == "AccountID")?.Value, out int parsedAccountId) ? parsedAccountId : 0;

            var message = "";
            var newsarticle = _newsArticleRepository.GetNewsArticle((int)id, 0, out message);

            if (newsarticle != null)
            {
                NewsArticle = newsarticle;
                Comments = _commentRepository.GetComments((int)id, role, accountId, out message);
                return Page();
            }

            return NotFound();
        }



    }
}
