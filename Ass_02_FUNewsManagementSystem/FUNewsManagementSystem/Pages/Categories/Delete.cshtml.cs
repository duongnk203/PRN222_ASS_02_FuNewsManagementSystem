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

            return NotFound();
        }

        public IActionResult OnPost(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var message = "";
            var category = _categoryRepository.GetCategory((int)id,out message);
            Category = category;

            _categoryRepository.Delete((int)id, out message);
            if(!string.IsNullOrEmpty(message))
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
