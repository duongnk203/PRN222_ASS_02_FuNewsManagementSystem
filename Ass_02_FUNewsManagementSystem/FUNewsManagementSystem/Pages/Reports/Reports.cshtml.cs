using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Reports
{
    public class ReportsModel : PageModel
    {
        private readonly IReportRepository _reportRepository;

        public ReportsModel(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public List<NewsArticle> ReportData { get; set; } = new List<NewsArticle>();

        public void OnGet()
        {
            // Khi trang load lần đầu tiên, không hiển thị dữ liệu
        }

        public IActionResult OnPost()
        {
            if (StartDate > EndDate)
            {
                ModelState.AddModelError("", "Start date must be before end date.");
                return Page();
            }

            ReportData = _reportRepository.GenerateReport(StartDate, EndDate);
            return Page();
        }
    }
}
