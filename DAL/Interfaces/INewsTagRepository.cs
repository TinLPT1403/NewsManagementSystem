using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface INewsTagRepository : IGenericRepository<NewsTag>
    {
        Task<List<Tag>> GetTagsFromArticleAsync(string articleId);
        Task<List<NewsArticle>> GetArticlesFromTagAsync(int tagId);
    }
}
