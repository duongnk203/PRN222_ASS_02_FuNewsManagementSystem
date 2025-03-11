using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;

namespace FUNewsManagementSystem.Pages_Tags
{
    public class DeleteModel : PageModel
    {
        private readonly ITagRepository _tagRepository;

        public DeleteModel(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = "";
            var tag = _tagRepository.GetTag(id ?? 0, out message);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }
            

            if (tag is not null)
            {
                Tag = tag;

                return Page();
            }

            return NotFound();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var message = "";
            _tagRepository.Delete(id ?? 0, out message);
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
