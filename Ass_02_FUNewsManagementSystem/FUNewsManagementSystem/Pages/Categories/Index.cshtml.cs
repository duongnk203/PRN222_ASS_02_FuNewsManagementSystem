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
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public IndexModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IList<Category> Category { get;set; } = default!;

        public void OnGet()
        {
            var message = "";
            Category = _categoryRepository.GetCategories(out message);
            ViewData["ParentCategoryId"] = _categoryRepository.GetCategories(out message);
        }
    }
}
