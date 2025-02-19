using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    // Create DTO (for creating a new article)
    public class NewsArticleCreateDTO
    {
        public string NewsTitle { get; set; }

        [Required]
        public string Headline { get; set; }

        public string NewsContent { get; set; }

        public string NewsSource { get; set; }

        public bool NewsStatus { get; set; } = true;

        public int CategoryId { get; set; } = 0; // Category is required when creating

        public List<int> NewsTagIds { get; set; } = new();
    }

    // Update DTO (for updating an existing article)
    public class NewsArticleUpdateDTO
    {
        public string? NewsTitle { get; set; }  // No [Required], only if updated
        public string? Headline { get; set; }   // No [Required], only if updated
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public bool? NewsStatus { get; set; }  // Optional, can be left null if no update
        public int? CategoryId { get; set; }  // Optional, can be null if not updating
        public List<int>? NewsTagIds { get; set; } = new();
    }


}
