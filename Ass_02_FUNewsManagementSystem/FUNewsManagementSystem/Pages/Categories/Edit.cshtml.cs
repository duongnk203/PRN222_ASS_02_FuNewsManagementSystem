using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public EditModel(ICategoryRepository categoryRepository)
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
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
           ViewData["ParentCategoryId"] = new SelectList(_categoryRepository.GetCategories(out message), "CategoryId", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var message = "";

            _categoryRepository.Update(Category.CategoryId, Category, out message);

            return RedirectToPage("./Index");
        }

    }
}
