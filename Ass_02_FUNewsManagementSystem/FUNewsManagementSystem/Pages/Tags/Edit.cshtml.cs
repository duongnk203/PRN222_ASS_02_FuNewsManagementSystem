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

namespace FUNewsManagementSystem.Pages_Tags
{
    public class EditModel : PageModel
    {
        private readonly ITagRepository _tagRepository;

        public EditModel(ITagRepository tagRepository)
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
            if (tag == null || !string.IsNullOrEmpty(message))
            {
                return NotFound();
            }
            Tag = tag;
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
            _tagRepository.Update(Tag.TagId, Tag, out message);
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
