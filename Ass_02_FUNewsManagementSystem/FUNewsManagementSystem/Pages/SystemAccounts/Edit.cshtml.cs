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

namespace FUNewsManagementSystem.Pages_SystemAccounts
{
    public class EditModel : PageModel
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public EditModel(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public IActionResult OnGet(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var message = "";
            var systemaccount = _systemAccountRepository.GetAccount((int)id, out message);
            if (!string.IsNullOrEmpty(message))
            {
                return NotFound();
            }
            SystemAccount = systemaccount;
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
            _systemAccountRepository.UpdateAccount((int)SystemAccount.AccountId, SystemAccount, out message);
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
