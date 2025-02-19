using BLL.DTOs;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface INewsArticleService
    {
        Task CreateNewsArticleAsync(NewsArticleCreateDTO dto);
        Task UpdateNewsArticleAsync(string id, NewsArticleUpdateDTO dto);
        Task DeactiveNewsArticleAsync(string id);
        Task<NewsArticle> GetNewsArticleByIdAsync(string id);
        Task<IEnumerable<NewsArticle>> GetActiveNewsArticlesAsync();
        Task<IEnumerable<NewsArticle>> GetNewsArticlesByUserIdAsync(int userId);
    }
}
