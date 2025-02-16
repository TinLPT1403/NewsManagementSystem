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
    public class NewsTagService : INewsTagService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NewsTagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddNewsTag(string newsArticleId, int tagId)
        {
            var article = await _unitOfWork.NewsArticles.GetByIdAsync(newsArticleId);
            var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
            if (article == null)
            {
                throw new KeyNotFoundException($"NewsArticle with ID '{newsArticleId}' not found.");
            }
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID '{tagId}' not found.");
            }

            var newsTag = new NewsTag
            {
                NewsArticleId = newsArticleId,
                TagId = tagId
            };
            await _unitOfWork.NewsTags.AddAsync(newsTag);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteNewsTag(string newsArticleId, int tagId)
        {
            var article = await _unitOfWork.NewsArticles.GetByIdAsync(newsArticleId);
            var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
            if (article == null)
            {
                throw new KeyNotFoundException($"Article with ID {newsArticleId} not found");
            }
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tagId} does not exist");
            }
            var newsTag = await _unitOfWork.NewsTags.FirstOrDefaultAsync(nt => nt.NewsArticleId == newsArticleId && nt.TagId == tagId);

            if (newsTag == null)
            {
                throw new InvalidOperationException($"Tag with ID {tagId} is not associated with the article {newsArticleId}");
            }
            await _unitOfWork.NewsTags.DeleteAsync(newsTag);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<List<Tag>> GetTagsOfArticle(string newsArticleId)
        {
            var article = await _unitOfWork.NewsArticles.GetByIdAsync(newsArticleId);
            if (article == null)
            {
                throw new KeyNotFoundException($"Article with ID {newsArticleId} not found");
            }
            var tags = await _unitOfWork.NewsTags.GetTagsFromArticleAsync(newsArticleId);
            if (!tags.Any())
            {
                throw new KeyNotFoundException($"This article has no tags");
            }
            return tags;
        }

        public async Task<List<NewsArticle>> GetArticlesFromTag(int tagId)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tagId} not found");
            }
            var articles = await _unitOfWork.NewsTags.GetArticlesFromTagAsync(tagId);
            if(articles == null)
            {
                throw new KeyNotFoundException("No article associated with this tag");
            }
            return articles;

        }
    }
}
