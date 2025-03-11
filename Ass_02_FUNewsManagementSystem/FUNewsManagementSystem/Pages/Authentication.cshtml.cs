using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.ViewModel;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages
{
    public class AuthenticationModel : PageModel
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public AuthenticationModel(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }
        public void OnGet()
        {
            Page();
        }

        [BindProperty]
        public AccountLogin AccountLogin { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            var (role, message) = await _systemAccountRepository.Login(AccountLogin, HttpContext);

            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            return role switch
            {
                1 => RedirectToPage("/Categories/Index"), // Staff
                2 => RedirectToPage("Index", "NewsArticles", new { area = "Lecturer" }), // Lecture
                3 => RedirectToPage("/SystemAccounts/Index"), // Admin
                _ => NotFound()
            };
        }

        public async Task<IActionResult> OnPostLogout()
        {
            var message = await _systemAccountRepository.Logout(HttpContext);
            if (!string.IsNullOrEmpty(message))
            {
                TempData["Message"] = message;
                return Page();
            }
            return RedirectToPage("Index", "NewsArticles");
        }

        public IActionResult AccessDenied()
        {
            return Page();
        }
    }
}
