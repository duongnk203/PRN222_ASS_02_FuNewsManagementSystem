using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public IList<SystemAccount> SystemAccount { get; set; } = new List<SystemAccount>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; } // Nhận giá trị tìm kiếm từ URL

        public void OnGet()
        {
            var message = "";
            SystemAccount = _systemAccountRepository.GetAccounts(out message);

            // Nếu có từ khóa tìm kiếm, lọc danh sách tài khoản
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                SystemAccount = SystemAccount
                    .Where(a => a.AccountName.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                                a.AccountEmail.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
    }
}
