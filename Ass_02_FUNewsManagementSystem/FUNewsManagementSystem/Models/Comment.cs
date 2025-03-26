using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FUNewsManagementSystem.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int NewsArticleId { get; set; }
    public short AccountId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }
    public virtual SystemAccount Account { get; set; } = null!;
    public virtual NewsArticle NewsArticle { get; set; } = null!;
}
