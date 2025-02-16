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
    }
}
