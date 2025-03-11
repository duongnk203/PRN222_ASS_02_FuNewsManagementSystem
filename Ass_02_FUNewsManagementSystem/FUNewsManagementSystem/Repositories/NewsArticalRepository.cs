using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.ViewModel;
using System.Runtime.InteropServices;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FUNewsManagementSystem.Models;

namespace FUNewsManagementSystem.Repositories
{
    public interface ITagRepository
    {
        void Create(Tag newsTag, out string message);
        void Delete(int id, out string message);
        Tag GetTag(int id, out string message);
        List<Tag> GetTags(out string message);
        void Update(int? id, Tag tagUpdate, out string message);
    }

    public class TagRepository : ITagRepository
    {
        private readonly FunewsManagementContext _context;
        public TagRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public void Create(Tag newsTag, out string message)
        {
            message = "";
            if (newsTag == null)
            {
                message = "Tag is not create!";
                return;
            }
            if (IsExistTag(newsTag.TagName))
            {
                message = "Tag name is exist!";
                return;
            }
            _context.Tags.Add(newsTag);
            _context.SaveChanges();
        }

        public void Delete(int id, out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "TagId is not exist!";
                return;
            }
            var tag = GetTag(id, out message);
            if (tag == null || !message.IsNullOrEmpty())
            {
                message = "Tag is not exist!";
                return;
            }
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }

        public Tag GetTag(int id, out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "TagId is invalid";
            }
            var tag = _context.Tags.FirstOrDefault(x => x.TagId == id);
            if (tag == null)
            {
                message = "Tag is not exist!";
            }
            return tag;
        }

        public bool IsExistTag(string tagName)
        {
            return _context.Tags.FirstOrDefault(t => t.TagName == tagName) != null;
        }

        public List<Tag> GetTags(out string message)
        {
            message = "";
            var tags = _context.Tags.ToList();
            if (tags.Count == 0)
            {
                message = "The list tag is empty!";
            }
            return tags;
        }

        public void Update(int? id, Tag tagUpdate, out string message)
        {
            message = "";
            if (id == 0 || tagUpdate == null)
            {
                message = "Tag is not exist!";
                return;
            }
            if (tagUpdate != null)
            {
                var tag = _context.Tags.FirstOrDefault(x => x.TagId == id);
                if (tag == null)
                {
                    message = "Tag not found";
                    return;
                }
                if (IsExistTag(tagUpdate.TagName) && !tag.TagName.Equals(tagUpdate.TagName))
                {
                    message = "Tag name is exist";
                    return;
                }

                // Cập nhật các thuộc tính của tag từ tagUpdate
                tag.TagName = tagUpdate.TagName;
                tag.Note = tagUpdate.Note;
                // Cập nhật các thuộc tính khác nếu cần

                _context.Update(tag); // Không cần thiết, tag đã được theo dõi và thay đổi
                _context.SaveChanges();
            }
        }
    }

    public interface INewsArticalRepository
    {
        void Create(int accountID, NewsArticle newsArticle, out string message);
        void Update(int id, int accountId, NewsArticle newsArticleUpdate, out NewsArticle newsArticleAfterUpdate, out string message);
        void Delete(int id, out string message);
        List<NewsArticleViewModel> GetNewsArticles(int role, out string message);
        NewsArticle GetNewsArticle(int id, int role, out string message);
        List<NewsArticleViewModel> GetNewsArticlesByCreated(int createdId, out string message);
    }

    public class NewsArticalRepository : INewsArticalRepository
    {
        private readonly FunewsManagementContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NewsArticalRepository(FunewsManagementContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Create(int accountID, NewsArticle newsArticle, out string message)
        {
            message = "";
            if (newsArticle == null)
            {
                message = "News Article is not created!";
                return;
            }

            // Gán thông tin người tạo
            newsArticle.CreatedById = (short)accountID;
            newsArticle.CreatedDate = DateTime.Now;

            // Thêm vào database
            _context.NewsArticles.Add(newsArticle);
            _context.SaveChanges(); // Lúc này ID sẽ được cập nhật

            // Tạo link chi tiết của bài viết
            string articleLink = $"https://localhost:7135/NewsArticleView/Details?id={newsArticle.NewsArticleId}";

            //// Nếu bài viết được publish (NewsStatus == true) thì gửi email
            //if (newsArticle.NewsStatus == true)
            //{
            //    string emailReceiver = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            //    _emailService.SendEmail(
            //        emailReceiver,
            //        $"{newsArticle.NewsTitle}",
            //        $"Author ID: {newsArticle.CreatedByID}. <br> Click <a href='{articleLink}'>here</a> to view the article.",
            //        articleLink
            //    );
            //}
        }


        public void Delete(int id, out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "News article is not exist!";
                return;
            }
            var newsArticle = GetNewsArticle(id, GetUserRole(), out message);
            if (newsArticle == null || !message.IsNullOrEmpty())
            {
                return;
            }
            _context.NewsArticles.Remove(newsArticle);
            _context.SaveChanges();
        }

        public NewsArticle GetNewsArticle(int id, int role, out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "News Article id is not exist!";
            }
            NewsArticle newsArticle = new NewsArticle();
            if (role != 1) newsArticle = _context.NewsArticles.Include(n => n.Category).Include(n=> n.CreatedBy).FirstOrDefault(x => x.NewsArticleId.Equals(id) && x.NewsStatus == true);
            else newsArticle = _context.NewsArticles.Include(n => n.Category).Include(n => n.CreatedBy).FirstOrDefault(x => x.NewsArticleId.Equals(id));
            if (newsArticle == null)
            {
                message = "News Article id is not exist!";
            }
            return newsArticle;
        }

        public List<NewsArticleViewModel> GetNewsArticles(int role, out string message)
        {
            message = "";
            try
            {
                var query = (from news in _context.NewsArticles
                             join updatedBy in _context.SystemAccounts
                             on news.UpdatedById equals updatedBy.AccountId into updatedByJoin
                             from updatedBy in updatedByJoin.DefaultIfEmpty() // Đảm bảo không bị null
                             select new NewsArticleViewModel
                             {
                                 NewsArticleID = news.NewsArticleId,
                                 NewsTitle = news.NewsTitle,
                                 Headline = news.Headline,
                                 CreatedDate = news.CreatedDate,
                                 NewsSource = news.NewsSource,
                                 NewsStatus = news.NewsStatus,
                                 ModifiedDate = news.ModifiedDate,
                                 CategoryID = news.CreatedById,
                                 CategoryName = news.Category.CategoryName,
                                 CreatedByName = news.CreatedBy.AccountName,
                                 CreatedByID = news.CreatedById,
                                 UpdatedByName = updatedBy != null ? updatedBy.AccountName : "N/A",
                             }).ToList();

                // Nếu role là 2 (Lecture), chỉ lấy bài viết có NewsStatus == true
                if (role == 2 || role == 0)
                {
                    query = query.Where(x => x.NewsStatus == true).ToList();
                }

                if (!query.Any())
                {
                    message = "The list of news articles is empty.";
                }

                return query;
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}";
                return new List<NewsArticleViewModel>(); // Trả về danh sách rỗng nếu có lỗi
            }
        }



        public void Update(int id, int accountId, NewsArticle newsArticleUpdate, out NewsArticle newsArticleAfterUpdate, out string message)
        {
            newsArticleAfterUpdate = newsArticleUpdate;
            message = "";
            if (id == 0)
            {
                message = "News article is not exist!";
                return;
            }
            var newsArticle = GetNewsArticle(id, GetUserRole(), out message);
            if (!message.IsNullOrEmpty() || newsArticle == null)
            {
                return;
            }

            if (IsExitNewTitle(newsArticleUpdate.NewsTitle) && !newsArticleUpdate.NewsTitle.Equals(newsArticle.NewsTitle))
            {
                message = "Title is exist!";
                return;
            }
            newsArticle.Headline = newsArticleUpdate.Headline;
            newsArticle.NewsContent = newsArticleUpdate.NewsContent;
            if (_context.Categories.FirstOrDefault(x => x.CategoryId == newsArticleUpdate.CategoryId).IsActive == true)
            {
                newsArticle.NewsStatus = newsArticleUpdate.NewsStatus;

            }
            newsArticle.NewsSource = newsArticleUpdate.NewsSource;
            newsArticle.NewsTitle = newsArticleUpdate.NewsTitle;
            //newsArticle.NewsTags = newsArticleUpdate.NewsTags;
            newsArticle.UpdatedById = (short)accountId;
            newsArticle.CategoryId = newsArticleUpdate.CategoryId;
            newsArticle.ModifiedDate = DateTime.Now;
            _context.Update(newsArticle);
            _context.SaveChanges();
            newsArticleAfterUpdate = newsArticleUpdate;
        }
        public bool IsExitNewTitle(string title)
        {
            return _context.NewsArticles.FirstOrDefault(x => x.NewsTitle.Equals(title)) != null;
        }

        public List<NewsArticleViewModel> GetNewsArticlesByCreated(int id, out string message)
        {
            message = "";

            List<NewsArticleViewModel> newsArticles = (
                from news in _context.NewsArticles
                join updatedBy in _context.SystemAccounts
                on news.UpdatedById equals updatedBy.AccountId into updatedByJoin
                from updatedBy in updatedByJoin.DefaultIfEmpty()
                where news.CreatedById == id // 🔥 THÊM ĐIỀU KIỆN LỌC
                select new NewsArticleViewModel
                {
                    NewsArticleID = news.NewsArticleId,
                    NewsTitle = news.NewsTitle,
                    Headline = news.Headline,
                    CreatedDate = news.CreatedDate,
                    NewsSource = news.NewsSource,
                    NewsStatus = news.NewsStatus,
                    ModifiedDate = news.ModifiedDate,
                    CategoryID = news.CategoryId,
                    CategoryName = news.Category.CategoryName,
                    CreatedByName = news.CreatedBy.AccountName,
                    CreatedByID = news.CreatedById,
                    UpdatedByName = updatedBy != null ? updatedBy.AccountName : "N/A",
                }
            ).ToList();

            if (newsArticles.Count == 0)
            {
                message = "The list news article is empty";
            }

            return newsArticles;
        }
        private int GetUserRole()
        {
            var user = _httpContextAccessor.HttpContext?.User; // Lấy thông tin User
            return user?.GetUserRole() ?? 0; // Dùng Extension Method đã tạo
        }


    }

}
