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
    public class DetailsModel : PageModel
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public DetailsModel(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

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
    }
}
