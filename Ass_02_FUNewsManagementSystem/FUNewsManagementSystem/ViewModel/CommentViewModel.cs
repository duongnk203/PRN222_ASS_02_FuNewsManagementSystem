using FUNewsManagementSystem.Models;
using System.Security.Cryptography.Pkcs;

namespace FUNewsManagementSystem.ViewModel
{
    public class CreateCommentViewModel
    {
        public int NewsArticleId { get; set; }
        public string Content { get; set; }
    }

    public class UpdateComment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
    }

    public class CommentDisplay
    {
        public Comment Comment { get; set; }
        public bool canUpdate { get; set; }
        public bool canDelete { get; set; }
    }
}
