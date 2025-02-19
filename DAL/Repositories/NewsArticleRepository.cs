using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        private readonly NewsContext newsContext;
        public NewsArticleRepository(NewsContext context) : base(context)
        {
            newsContext = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllByCategoryIdAsync(int id)
        {
            return await newsContext.NewsArticles
                .Where(article => article.CategoryId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsArticlesAsync()
        {
            return await newsContext.NewsArticles
                .Include(article => article.Category)   // Include Category to access CategoryDescription
                .Include(article => article.CreatedBy)  // Include CreatedBy to access AccountId
                .Include(article => article.UpdatedBy)  // Include UpdatedBy to access AccountId
                .Where(article => article.NewsStatus == true)
                .ToListAsync();
        }
        public async Task<IEnumerable<NewsArticle>> GetActiveNewsArticlesByUserIdAsync(int userId)
        {
            return await newsContext.NewsArticles
                .Where(article => article.CreatedBy.AccountId == userId)
                .Include(article => article.Category)   // Include Category to access CategoryDescription
                .Include(article => article.CreatedBy)  // Include CreatedBy to access AccountId
                .Include(article => article.UpdatedBy)  // Include UpdatedBy to access AccountId
                .Where(article => article.NewsStatus == true)
                .ToListAsync();
        }

        public async Task<NewsArticle> GetNewsArticleByIdAsync(string id)
        {
            return await newsContext.NewsArticles
                .Include(article => article.Category)   // Include Category to access CategoryDescription
                .Include(article => article.CreatedBy)  // Include CreatedBy to access AccountId
                .Include(article => article.UpdatedBy)  // Include UpdatedBy to access AccountId
                .FirstOrDefaultAsync(article => article.NewsArticleId == id);
        }

    }
}
