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
        Task AddNewsTag(string NewsArticleId, int TagId);
        Task<List<Tag>> GetTagsOfArticle(string NewsArticleId);
        Task<List<NewsArticle>> GetArticlesFromTag(int TagId);
        Task DeleteNewsTag(string NewsArticleId, int TagId);
    }
}
