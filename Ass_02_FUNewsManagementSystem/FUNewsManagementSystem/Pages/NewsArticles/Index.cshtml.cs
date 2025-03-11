using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Repositories;
using FUNewsManagementSystem.ViewModel;
using FUNewsManagementSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Pages_NewsAriticles
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticalRepository _newsArticleRepository;

        public IndexModel(INewsArticalRepository newsArticalRepository)
        {
            _newsArticleRepository = newsArticalRepository;
        }

        public string SearchString { get; set; } = string.Empty; // Gán giá trị mặc định
        public List<NewsArticleViewModel> NewsArticle { get; set; } = new List<NewsArticleViewModel>();

        public void OnGet(string searchString, bool? history)
        {
            try
            {
                var roleClaim = User.FindFirst(ClaimTypes.Role);
                int role = (roleClaim == null || string.IsNullOrEmpty(roleClaim.Value)) ? 0 : int.TryParse(roleClaim.Value, out var parsedRole) ? parsedRole : 0;

                var message = "";
                var newsArticles = _newsArticleRepository.GetNewsArticles(role, out message);

                if (!string.IsNullOrEmpty(message))
                {
                    TempData["Message"] = $"Error fetching news articles: {message}";
                }

                // Gán giá trị searchString để hiển thị lại trong ô tìm kiếm
                SearchString = searchString;

                // Lọc theo từ khóa tìm kiếm
                if (!string.IsNullOrEmpty(searchString))
                {
                    newsArticles = newsArticles
                        .Where(n => (n.NewsTitle?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false)
                                 || (n.Headline?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();
                }

                // Lọc theo bài báo do người dùng hiện tại tạo
                if (history == true)
                {
                    var accountIdClaim = User.FindFirst("AccountID")?.Value;
                    if (!string.IsNullOrEmpty(accountIdClaim) && int.TryParse(accountIdClaim, out int accountId))
                    {
                        newsArticles = newsArticles.Where(n => n.CreatedByID == (short)accountId).ToList();
                    }
                }

                // Cập nhật danh sách hiển thị
                NewsArticle = newsArticles;
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Exception in OnGet: {ex.Message}";
                NewsArticle = new List<NewsArticleViewModel>();
            }
        }

    }
}
