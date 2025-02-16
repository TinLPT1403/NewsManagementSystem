using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class NewsTag
    {
        [Key]
        [Required]
        public string NewsArticleId { get; set; } = Guid.NewGuid().ToString();
        public virtual NewsArticle NewsArticle { get; set; }
        [Required]
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; } // Navigation to Tag
    }
}
