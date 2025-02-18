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
        Task<List<Tag>> GetTagsOfArticleAsync(string NewsArticleId);
        Task<List<NewsArticle>> GetArticlesFromTagAsync(int TagId);
        Task DeleteNewsTagAsync(string NewsArticleId, int TagId);
    }
}
