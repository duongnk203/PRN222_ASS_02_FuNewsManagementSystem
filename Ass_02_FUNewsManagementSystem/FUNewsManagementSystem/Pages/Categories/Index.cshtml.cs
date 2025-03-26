using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public IndexModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IList<Category> Category { get; set; } = new List<Category>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; } // Thêm thuộc tính để nhận từ khóa tìm kiếm

        public void OnGet()
        {
            var message = "";
            Category = _categoryRepository.GetCategories(out message);

            // Nếu có từ khóa tìm kiếm, lọc danh mục
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Category = Category
                    .Where(c => c.CategoryName.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewData["ParentCategoryId"] = _categoryRepository.GetCategories(out message);
        }
    }
}
