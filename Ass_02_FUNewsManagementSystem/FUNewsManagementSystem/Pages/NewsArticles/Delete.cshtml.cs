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
    public class DeleteModel : PageModel
    {
        private readonly INewsArticalRepository _newsArticleRepository;

        public DeleteModel(INewsArticalRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var message = "";
            var newsarticle = _newsArticleRepository.GetNewsArticle((int)id, 0, out message);

            if (newsarticle is not null)
            {
                NewsArticle = newsarticle;

                return Page();
            }
            

            return NotFound();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = "";
            _newsArticleRepository.Delete((int)id, out message);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
