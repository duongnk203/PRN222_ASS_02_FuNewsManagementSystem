using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.Repositories;
using FUNewsManagementSystem.Pages.Tags;

namespace FUNewsManagementSystem.Pages_Tags
{
    public class DetailsModel : PageModel
    {
        private readonly ITagRepository _tagRepository;

        public DetailsModel(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public Tag Tag { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var message = "";
            var tag = _tagRepository.GetTag(id ?? 0, out message);

            if (tag is not null || !string.IsNullOrEmpty(message))
            {
                Tag = tag;
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            return NotFound();
        }
    }
}
