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
    public class NewsTagRepository : GenericRepository<NewsTag>, INewsTagRepository
    {
        private readonly NewsContext newsContext;
        public NewsTagRepository(NewsContext context) : base(context)
        {
            newsContext = context;
        }

        public async Task<List<Tag>> GetTagsFromArticleAsync(string articleId)
        {
            return await newsContext.NewsTags
                                    .Where(nt => nt.NewsArticleId == articleId)
                                    .Select(nt => nt.Tag)
                                    .ToListAsync();
        }

        public async Task<List<NewsArticle>> GetArticlesFromTagAsync(int tagId)
        {
            return await newsContext.NewsTags
                                    .Where(nt => nt.TagId == tagId)
                                    .Select(nt => nt.NewsArticle)
                                    .ToListAsync();
        }
    }
}
