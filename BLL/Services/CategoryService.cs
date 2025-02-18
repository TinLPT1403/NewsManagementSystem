using BLL.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null) throw new KeyNotFoundException("Category not found.");

            existingCategory.CategoryName = category.CategoryName;
            existingCategory.CategoryDescription = category.CategoryDescription;
            existingCategory.ParentCategoryId = category.ParentCategoryId;

            await _unitOfWork.Categories.UpdateAsync(existingCategory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("Category not found.");

            // Check if the category is associated with any news articles
            if ((await _unitOfWork.NewsArticles.GetAllByCategoryIdAsync(id)).Any()) throw new InvalidOperationException("Cannot delete category associated with news articles.");

            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}