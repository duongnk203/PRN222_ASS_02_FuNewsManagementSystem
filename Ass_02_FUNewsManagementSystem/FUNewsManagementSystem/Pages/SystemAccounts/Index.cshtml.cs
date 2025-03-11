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
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public IndexModel(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        public IList<SystemAccount> SystemAccount { get;set; } = default!;

        public void OnGet()
        {
            var message = "";
            SystemAccount = _systemAccountRepository.GetAccounts(out message);
        }
    }
}
