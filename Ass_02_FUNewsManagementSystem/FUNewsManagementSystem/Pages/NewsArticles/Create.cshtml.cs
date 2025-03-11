using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_NewsAriticles
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticalRepository _newsArticalRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;

        public CreateModel(INewsArticalRepository newsArticalRepository, ICategoryRepository categoryRepository, ISystemAccountRepository systemAccountRepository)
        {
            _newsArticalRepository = newsArticalRepository;
            _categoryRepository = categoryRepository;
            _systemAccountRepository = systemAccountRepository;
        }

        public IActionResult OnGet()
        {
            var message = "";

            // Lấy danh sách danh mục và đặt vào ViewData
            var categories = _categoryRepository.GetCategories(out message);
            if (!string.IsNullOrEmpty(message))
            {
                TempData["Message"] = message;
                return Page();
            }
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");

            // Lấy danh sách tài khoản và đặt vào ViewData
            var accounts = _systemAccountRepository.GetAccounts(out message);
            if (!string.IsNullOrEmpty(message))
            {
                TempData["Message"] = message;
                return Page();
            }
            ViewData["CreatedById"] = new SelectList(accounts, "AccountId", "FullName");

            return Page();
        }


        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var message = "";

            var accountID = Int32.Parse(User.FindFirst("AccountID").Value);
            _newsArticalRepository.Create(accountID, NewsArticle, out message);

            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
