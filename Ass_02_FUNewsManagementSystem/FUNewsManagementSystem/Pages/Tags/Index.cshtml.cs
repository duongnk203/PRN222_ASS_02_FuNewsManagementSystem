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
    public class IndexModel : PageModel
    {
        private readonly ITagRepository _tagRepository;

        public IndexModel(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IList<Tag> Tag { get;set; } = default!;

        public void OnGet()
        {
            var message = "";
            Tag = _tagRepository.GetTags(out message);
        }
    }
}
