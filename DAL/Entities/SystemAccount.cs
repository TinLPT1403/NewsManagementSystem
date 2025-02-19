using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class SystemAccount
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Mark as auto-increment (identity)
        public int AccountId { get; set; }
        public required string AccountName { get; set; }
        public required string AccountEmail { get; set; }
        public required int AccountRole { get; set; }
        public required string AccountPassword { get; set; }


        public virtual ICollection<NewsArticle> CreatedArticles { get; set; } = new List<NewsArticle>(); // Reverse navigation
        public virtual ICollection<NewsArticle> UpdatedArticles { get; set; } = new List<NewsArticle>(); // Reverse navigation

    }
}
