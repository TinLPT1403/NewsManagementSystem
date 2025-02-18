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
        Task DeleteNewsArticleAsync(string id);
        Task<NewsArticle> GetNewsArticleAsync(string id);
        Task<List<NewsArticle>> GetAllNewsArticlesAsync();
        Task<string?> GetNewsArticlesByUserIdAsync(int userId);
    }
}
