using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Models;

namespace FUNewsManagementSystem.Repositories
{
    public interface IReportRepository
    {
        List<NewsArticle> GenerateReport(DateTime startDate, DateTime endDate);
    }
    public class ReportRepository : IReportRepository
    {
        private readonly FunewsManagementContext _context;
        public ReportRepository(FunewsManagementContext context)
        {
            _context = context;
        }
        public List<NewsArticle> GenerateReport(DateTime startDate, DateTime endDate)
        {

            return _context.NewsArticles
                            .Include(n => n.CreatedBy)
                          .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                          .OrderByDescending(n => n.CreatedDate)
                          .ToList();

        }
    }
}
