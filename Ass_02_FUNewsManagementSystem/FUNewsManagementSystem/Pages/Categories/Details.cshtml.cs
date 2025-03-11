using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_Categories
{
    public class DetailsModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public DetailsModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Category Category { get; set; } = default!;

        public IActionResult OnGetAsync(short? id)
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
    }
}
