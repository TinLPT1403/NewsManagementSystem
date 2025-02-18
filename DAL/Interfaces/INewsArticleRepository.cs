using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<IEnumerable<NewsArticle>> GetAllByCategoryIdAsync(int id);
        Task<IEnumerable<NewsArticle>> GetNewsArticlesAsync();
    }
}
