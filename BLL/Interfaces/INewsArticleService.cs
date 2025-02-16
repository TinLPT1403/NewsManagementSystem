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
        Task CreateNewsArticle(NewsArticleCreateDTO dto);
        Task UpdateNewsArticle(string id, NewsArticleUpdateDTO dto);
        Task DeleteNewsArticle(string id);
        Task<NewsArticle> GetNewsArticle(string id);
        Task<List<NewsArticle>> GetAllNewsArticles();
        Task<string?> GetNewsArticlesByUserId(int userId);
    }
}
