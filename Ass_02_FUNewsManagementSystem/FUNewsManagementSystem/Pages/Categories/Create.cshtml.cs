using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryRepository _categorRepository;

        public CreateModel(ICategoryRepository categoryRepository)
        {
            _categorRepository = categoryRepository;
        }

        public IActionResult OnGet()
        {
            var message = "";
        ViewData["ParentCategoryId"] = new SelectList(_categorRepository.GetCategories(out message), "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var message = "";

            _categorRepository.Create(Category, out message);
            return RedirectToPage("./Index");
        }
    }
}
