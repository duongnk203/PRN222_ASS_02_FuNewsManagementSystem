using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;
using FUNewsManagementSystem.Service;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FUNewsManagementSystem.Pages.Profile
{
    public class EditModel : PageModel
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly RoleService _roleService;

        public EditModel(ISystemAccountRepository systemAccountRepository, RoleService roleService)
        {
            _systemAccountRepository = systemAccountRepository;
            _roleService = roleService;
        }

        [BindProperty]
        public SystemAccount Account { get; set; }

        public List<SelectListItem> RoleList { get; set; }

        public IActionResult OnGet()
        {
            // Lấy AccountID từ Claims
            var accountIdClaim = User.FindFirst("AccountID")?.Value;
            if (string.IsNullOrEmpty(accountIdClaim))
            {
                return RedirectToPage("/Authentication/AccessDenied");
            }

            int accountID = int.Parse(accountIdClaim);
            string message;
            Account = _systemAccountRepository.GetAccount(accountID, out message);

            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            // Lấy danh sách vai trò
            RoleList = _roleService.GetRoles();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                RoleList = _roleService.GetRoles();
                return Page();
            }

            int accountID = int.Parse(User.FindFirst("AccountID")?.Value);
            string message;
            _systemAccountRepository.UpdateAccount(accountID, Account, out message);

            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                RoleList = _roleService.GetRoles();
                return Page();
            }

            TempData["Message"] = "Account updated successfully.";
            return RedirectToPage("Details");
        }
    }
}
