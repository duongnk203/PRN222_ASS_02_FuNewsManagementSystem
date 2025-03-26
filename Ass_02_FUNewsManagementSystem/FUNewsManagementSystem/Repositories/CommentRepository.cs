using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace FUNewsManagementSystem.Repositories
{
    public interface ICommentRepository
    {
        List<CommentDisplay> GetComments(int newsArticleID, int role, int accountId, out string message);
        void CreateComment(Comment comment, out string message);
        void DeleteComment(int commentID, out string message);
        Comment GetComment(int commentId, out string message);
        void Update(int commentId, string content, out string message);
    }

    public class CommentRepository : ICommentRepository
    {
        private FunewsManagementContext _context;
        public CommentRepository(FunewsManagementContext context)
        {
            _context = context;
        }
        public void CreateComment(Comment comment, out string message)
        {
            message = "";
            if(comment == null)
            {
                message = "Create comment is failed";
                return;
            }
            _context.Add<Comment>(comment);
            _context.SaveChanges();

        }

        public void DeleteComment(int commentID, out string message)
        {
            message = "";
            if (commentID == 0)
            {
                message = "Comment is not exist!";
                return;
            }

            var comment = _context.Comments.FirstOrDefault(c => c.CommentId == commentID);
            if (comment == null)
            {
                message = "Comment is not exist!";
                return; 
            }
            comment.IsActive = false;
            _context.Update<Comment>(comment);
            _context.SaveChanges();
        }

        public Comment GetComment(int commentId, out string message)
        {
            throw new NotImplementedException();
        }

        public List<CommentDisplay> GetComments(int newsArticleID, int role, int accountId, out string message)
        {
            message = "";
            List<CommentDisplay> commentsDisplay = new List<CommentDisplay>();

            if (newsArticleID <= 0)
            {
                message = "News article does not exist for user comments!";
                return commentsDisplay;
            }

            var comments = _context.Comments
                .Where(c => c.NewsArticleId == newsArticleID && c.IsActive == true)
                .Include(c => c.Account)
                .ToList();

            if (comments.Count == 0)
            {
                message = "News article does not have comments";
            }

            // Nếu accountId == -1 hoặc không khớp với c.AccountId, thì không thể sửa hoặc xóa (trừ Admin)
            bool isValidUser = accountId > 0;
            bool isAdmin = role == 3;

            // Duyệt qua danh sách comment và gán quyền
            commentsDisplay = comments.Select(c => new CommentDisplay
            {
                Comment = c,
                canDelete = isAdmin || (isValidUser && (role == 1 || role == 2) && c.AccountId == accountId),
                canUpdate = isValidUser && (role == 1 || role == 2) && c.AccountId == accountId
            }).ToList();

            return commentsDisplay;
        }



        public void Update(int commentId, string content, out string message)
        {
            message = "";
            if (commentId == 0)
            {
                message = "Comment is not exist!";
                return;
            }

            var comment = _context.Comments.FirstOrDefault(c => c.CommentId == commentId && c.IsActive == true);
            if (comment == null)
            {
                message = "Comment is not exist!";
                return;
            }
            comment.Content = content;
            _context.Update<Comment>(comment);
            _context.SaveChanges();
        }
    }
}
