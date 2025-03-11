using System;
using System.Collections.Generic;

namespace FUNewsManagementSystem.Models;

public partial class SystemAccount
{
    public short AccountId { get; set; }

    public string AccountName { get; set; } = null!;

    public string AccountEmail { get; set; } = null!;

    public int AccountRole { get; set; }

    public string AccountPassword { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
