using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_NewsAriticles
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticalRepository _newsArticleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly SignalRServer _signalRServer;

        public DetailsModel(INewsArticalRepository newsArticalRepository, ICommentRepository commentRepository, SignalRServer signalRServer)
        {
            _newsArticleRepository = newsArticalRepository;
            _commentRepository = commentRepository;
            _signalRServer = signalRServer; 
        }

        public NewsArticle NewsArticle { get; set; } = default!;
        public List<Comment> Comments { get; set; } = new List<Comment>();

        [BindProperty]
        public int NewsArticleId { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = "";
            var newsarticle = _newsArticleRepository.GetNewsArticle((int)id, 0, out message);
            Comments = _commentRepository.GetComments(newsarticle.NewsArticleId, out message);

            if (newsarticle is not null)
            {
                NewsArticle = newsarticle;

                return Page();
            }

            return NotFound();
        }


    }
}
