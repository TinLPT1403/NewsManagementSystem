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

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string AccountName { get; set; }

        [Required]
        [EmailAddress]
        public string AccountEmail { get; set; }

        [Required]
        [Range(1, 2)]
        public int AccountRole { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string AccountPassword { get; set; }

        public virtual ICollection<NewsArticle> CreatedArticles { get; set; } = new List<NewsArticle>(); // Reverse navigation
        public virtual ICollection<NewsArticle> UpdatedArticles { get; set; } = new List<NewsArticle>(); // Reverse navigation
    }
}
