using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_Tags
{
    public class CreateModel : PageModel
    {
        private readonly ITagRepository _tagRepository;

        public CreateModel(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var message = "";
            _tagRepository.Update((int)Tag.TagId, Tag, out message);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
