using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<NewsArticle>> GetArticlesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Category>> GetActiveCategories();
        Task RemoveAsync(Category category);
    }
}
