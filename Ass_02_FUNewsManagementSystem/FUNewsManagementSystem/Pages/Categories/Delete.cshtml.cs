using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        [TempData] // Lưu trữ thông báo lỗi cho postback
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = "";
            var category = _categoryRepository.GetCategory((int)id, out message);

            if (category is not null)
            {
                Category = category;
                return Page();
            }

            ErrorMessage = "Category not found!";
            return RedirectToPage("./Index");
        }

        public IActionResult OnPost(short? id)
        {
            if (id == null)
            {
                ErrorMessage = "Invalid category ID!";
                return RedirectToPage("./Index");
            }

            var message = "";
            var category = _categoryRepository.GetCategory((int)id, out message);
            Category = category;
            if (category == null)
            {
                ErrorMessage = "Category not found!";
                return RedirectToPage("./Index");
            }

            _categoryRepository.Delete((int)id, out message);

            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
