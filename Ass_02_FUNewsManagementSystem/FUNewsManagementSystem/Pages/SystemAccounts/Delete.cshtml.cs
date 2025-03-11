using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_SystemAccounts
{
    public class DeleteModel : PageModel
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public DeleteModel(ISystemAccountRepository systemAccountRepository)
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

            if (systemaccount is not null)
            {
                SystemAccount = systemaccount;

                return Page();
            }

            return NotFound();
        }

        public IActionResult OnPost(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var message = "";
            var systemaccount = _systemAccountRepository.GetAccount((int)id, out message);
            if (systemaccount != null)
            {
                SystemAccount = systemaccount;
                _systemAccountRepository.DeleteAccount((int) id, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    ModelState.AddModelError(string.Empty, message);
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
