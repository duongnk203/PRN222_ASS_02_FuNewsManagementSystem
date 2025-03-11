using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;
using System.Security.Claims;

namespace FUNewsManagementSystem.Pages_NewsAriticles
{
    public class EditModel : PageModel
    {
        private readonly INewsArticalRepository _newsArticleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISystemAccountRepository _systemAccountRepository; 

        public EditModel(INewsArticalRepository newsArticalRepository, ICategoryRepository categoryRepository, ISystemAccountRepository systemAccountRepository)
        {
            _newsArticleRepository = newsArticalRepository;
            _categoryRepository = categoryRepository;
            _systemAccountRepository = systemAccountRepository;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            int role = (roleClaim == null || string.IsNullOrEmpty(roleClaim.Value)) ? 0 : int.TryParse(roleClaim.Value, out var parsedRole) ? parsedRole : 0;

            var message = "";
            var newsarticle = _newsArticleRepository.GetNewsArticle((int)id, role, out message);
            if (!string.IsNullOrEmpty(message))
            {
                return NotFound();
            }

            NewsArticle = newsarticle;

            // Lấy danh sách danh mục và hiển thị tên
            ViewData["CategoryId"] = new SelectList(
                _categoryRepository.GetCategories(out message),
                "CategoryId",
                "CategoryName"
            );

            // Lấy danh sách tài khoản và hiển thị tên người tạo
            ViewData["CreatedById"] = new SelectList(
                _systemAccountRepository.GetAccounts(out message),
                "AccountId",
                "FullName"
            );

            return Page();
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var message = "";
            NewsArticle newsArticle = new NewsArticle();
            _newsArticleRepository.Update(NewsArticle.NewsArticleId, Int32.Parse(User.FindFirst("AccountID").Value), NewsArticle, out newsArticle, out message);
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
