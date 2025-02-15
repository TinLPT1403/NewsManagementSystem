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
            var relatedArticle = await _unitOfWork.NewsArticles.FirstOrDefaultAsync(u => u.CategoryId == id);
            if (relatedArticle != null)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category != null)
                {
                    category.IsActive = false;
                    await _unitOfWork.Categories.UpdateAsync(category);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task<List<Category>> GetAllCategories() => await _unitOfWork.Categories.GetAllAsync();
        public async Task<Category> GetCategoryById(int id) => await _unitOfWork.Categories.GetByIdAsync(id);

        public async Task UpdateCategory(int id, CategoryDTO dto)
        {
            // Fetch the existing category by ID
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            // Check if the category exists
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            // Update CategoryName if provided (check for null or empty)
            if (!string.IsNullOrWhiteSpace(dto.CategoryName))
            {
                category.CategoryName = dto.CategoryName;
            }

            // Update CategoryDescription if provided (check for null or empty)
            if (!string.IsNullOrWhiteSpace(dto.CategoryDescription))
            {
                category.CategoryDescription = dto.CategoryDescription;
            }

            // No need to update IsActive since the check is not mentioned, assuming it's handled elsewhere

            // If ParentCategoryId is provided (and not 0), update it
            if (dto.ParentCategoryId.HasValue && dto.ParentCategoryId.Value != 0)
            {
                category.ParentCategoryId = dto.ParentCategoryId.Value;
            }

            // Update the category
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
