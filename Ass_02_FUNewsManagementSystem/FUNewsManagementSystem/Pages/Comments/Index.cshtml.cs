using FUNewsManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Comments
{
    public class IndexModel : PageModel
    {
        private readonly ICommentRepository _commentRepository;
        private readonly SignalRServer _signalRServer;
        public IndexModel(ICommentRepository commentRepository, SignalRServer signalRServer)
        {
            _commentRepository = commentRepository;
            _signalRServer = signalRServer;
        }
        //public void OnGet()
        //{
        //    var message = "";
        //    var comments = _commentRepository.GetComments()
        //}
    }
}
