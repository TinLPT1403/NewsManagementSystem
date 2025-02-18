using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class NewsArticle
    {
        [Key]
        [Required]
        public string NewsArticleId { get; set; } = Guid.NewGuid().ToString();
        public string? NewsTitle { get; set; }
        [Required]
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public bool? NewsStatus { get; set; } = true;
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedById { get; set; }
        public virtual SystemAccount? CreatedBy { get; set; } // Navigation for CreatedBy
        public int? UpdatedById { get; set; }
        public virtual SystemAccount? UpdatedBy { get; set; } // Navigation for UpdatedBy
        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>(); // Many-to-many relationship
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
