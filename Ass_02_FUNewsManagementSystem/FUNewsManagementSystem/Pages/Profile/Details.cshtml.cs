using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Service;
using System;
using System.Security.Claims;

namespace FUNewsManagementSystem.Pages.Profile
{
    public class DetailsModel : PageModel
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public DetailsModel(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        public SystemAccount Account { get; set; }

        public IActionResult OnGet()
        {
            // Lấy AccountID từ Claims
            var accountIdClaim = User.FindFirst("AccountID")?.Value;
            if (string.IsNullOrEmpty(accountIdClaim))
            {
                return RedirectToPage("/Authentication/AccessDenied");
            }

            int accountID = int.Parse(accountIdClaim);

            // Lấy thông tin tài khoản
            var message = "";
            Account = _systemAccountRepository.GetAccount(accountID, out message);

            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            return Page();
        }
    }
}
