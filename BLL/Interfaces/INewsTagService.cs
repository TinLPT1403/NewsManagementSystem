using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface INewsTagService
    {
        Task AddNewsTagAsync(string NewsArticleId, int TagId);
        Task<IEnumerable<Tag>> GetTagsOfArticleAsync(string NewsArticleId);
        Task<IEnumerable<NewsArticle>> GetArticlesFromTagAsync(int TagId);
        Task DeleteNewsTagAsync(string NewsArticleId, int TagId);
    }
}
