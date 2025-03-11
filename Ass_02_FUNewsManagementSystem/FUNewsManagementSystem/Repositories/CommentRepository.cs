using FUNewsManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace FUNewsManagementSystem.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> GetComments(int newsArticleID, out string message);
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

        public List<Comment> GetComments(int newsArticleID, out string message)
        {
            message = "";
            List<Comment> comments = new List<Comment>();

            if (newsArticleID <= 0)
            {
                message = "News article is not exist for user comment!";
                return comments;
            }

            comments = _context.Comments
                .Where(c => c.NewsArticleId == newsArticleID && c.IsActive == true)
                .Include(c => c.Account) // Include thông tin Account
                .ToList();

            if (comments.Count <= 0)
            {
                message = "News article does not have comments";
            }

            return comments;
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
