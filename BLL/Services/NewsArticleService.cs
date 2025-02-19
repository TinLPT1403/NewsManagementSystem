using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INewsArticleRepository _newsArticleRepository;

        public NewsArticleService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, INewsArticleRepository newsArticleRepository)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _newsArticleRepository = newsArticleRepository;
        }
        public async Task CreateNewsArticleAsync(NewsArticleCreateDTO dto)
        {
            // Ensure required fields are not empty
            if (string.IsNullOrWhiteSpace(dto.Headline))
            {
                throw new ArgumentException("Headline is required.");
            }
            var userId = GetUserFromToken();
            // Create new article
            var article = new NewsArticle
            {
                NewsTitle = dto.NewsTitle,  // Optional, can be null
                Headline = dto.Headline,
                CreatedDate = DateTime.UtcNow,
                NewsContent = dto.NewsContent,  // Optional
                NewsSource = dto.NewsSource,  // Optional
                NewsStatus = dto.NewsStatus,  // Default to true if not provided
                CategoryId = dto.CategoryId,
                ModifiedDate = DateTime.UtcNow,
                CreatedById = userId
            };

            // Add article to the repository
            await _unitOfWork.NewsArticles.AddAsync(article);

            // Handle tags if provided
            if (dto.NewsTagIds != null && dto.NewsTagIds.Any())
            {
                foreach (var tagId in dto.NewsTagIds)
                {
                    var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        var newsTag = new NewsTag
                        {
                            NewsArticleId = article.NewsArticleId,  // This assumes NewsArticleId is generated when saved
                            TagId = tagId,
                        };
                        await _unitOfWork.NewsTags.AddAsync(newsTag);
                    }
                }
            }

            // Save changes
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteNewsArticleAsync(string id)
        {
            var article = await _unitOfWork.NewsArticles.GetByIdAsync(id);
            if(article != null)
            {
                article.NewsStatus = false;
                await _unitOfWork.NewsArticles.UpdateAsync(article);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync() => await _unitOfWork.NewsArticles.GetAllAsync();


        public async Task<NewsArticle> GetNewsArticleAsync(string id) => await _unitOfWork.NewsArticles.GetByIdAsync(id);

        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesByUserIdAsync(int userId)
        {

           return await _newsArticleRepository.GetNewsArticlesByUserIdAsync(userId);
            
        }

        public async Task UpdateNewsArticleAsync(string id, NewsArticleUpdateDTO dto)
        {
            // Fetch the existing article by ID
            var article = await _unitOfWork.NewsArticles.GetByIdAsync(id);

            // Check if the article exists
            if (article == null)
            {
                throw new KeyNotFoundException("News article not found.");
            }

            // Update article properties based on DTO, only if the field is not null or empty
            if (!string.IsNullOrWhiteSpace(dto.NewsTitle))
            {
                article.NewsTitle = dto.NewsTitle;
            }

            if (!string.IsNullOrWhiteSpace(dto.Headline))
            {
                article.Headline = dto.Headline;
            }

            if (!string.IsNullOrWhiteSpace(dto.NewsContent))
            {
                article.NewsContent = dto.NewsContent;
            }

            if (!string.IsNullOrWhiteSpace(dto.NewsSource))
            {
                article.NewsSource = dto.NewsSource;
            }

            if (dto.NewsStatus.HasValue) // Only update if NewsStatus is provided
            {
                article.NewsStatus = dto.NewsStatus.Value;
            }

            // Only update ModifiedDate if there's any change in the article
            article.ModifiedDate = DateTime.UtcNow;

            // If CategoryId is provided and is different from the existing category, update it
            if (dto.CategoryId.HasValue && article.CategoryId != dto.CategoryId.Value)
            {
                article.CategoryId = dto.CategoryId.Value;
            }

            // Update tags if provided in the DTO (removing old ones and adding new ones)
            if (dto.NewsTagIds != null && dto.NewsTagIds.Any())
            {
                // Remove old tags associated with the article
                var existingTags = await _unitOfWork.NewsTags
                    .GetAllByListAsync(nt => nt.NewsArticleId == id);

                _unitOfWork.NewsTags.RemoveRange(existingTags);

                // Add new tags from the DTO
                foreach (var tagId in dto.NewsTagIds)
                {
                    var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        var newsTag = new NewsTag
                        {
                            NewsArticleId = article.NewsArticleId,  // Assuming this is the existing article's ID
                            TagId = tagId,
                        };
                        await _unitOfWork.NewsTags.AddAsync(newsTag);
                    }
                }
            }

            // Save the changes to the database
            await _unitOfWork.SaveChangesAsync();
        }

        private int GetUserFromToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return int.Parse(userIdClaim.Value);
        }
    }
}
