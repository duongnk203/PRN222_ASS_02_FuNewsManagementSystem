using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using FUNewsManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging;

namespace FUNewsManagementSystem.Pages_SystemAccounts
{
    //[Authorize(Roles ="Admin")]
    public class CreateModel : PageModel
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public CreateModel(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var message = "";
            _systemAccountRepository.CreateAccount(SystemAccount, out message);
            if(!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
