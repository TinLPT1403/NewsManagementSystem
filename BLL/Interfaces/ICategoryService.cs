using BLL.DTOs;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        Task CreateCategory(CategoryDTO dto);
        Task <Category> GetCategoryById(int id);
        Task <List<Category>> GetAllCategories();
        Task UpdateCategory(int id, CategoryDTO dto);
        Task DeleteCategory(int id);

    }
}
