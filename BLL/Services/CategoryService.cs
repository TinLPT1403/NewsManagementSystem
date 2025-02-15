using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCategory(CategoryDTO dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName,
                CategoryDescription = dto.CategoryDescription,
            };
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();   
        }

        public async Task DeleteCategory(int id)
        {
            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var categoryList = await _unitOfWork.Categories.GetAllAsync();
            return categoryList;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return category;
        }

        public async Task UpdateCategory(int id, CategoryDTO dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            category.CategoryName = dto.CategoryName;
            category.CategoryDescription = dto.CategoryDescription;
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
