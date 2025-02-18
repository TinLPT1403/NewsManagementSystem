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
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateTagAsync(TagDTO dto)
        {
            var tag = new Tag
            {
                TagName = dto.TagName,
                Note = dto.Note,
            };
            await _unitOfWork.Tags.AddAsync(tag);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(int TagId)
        {
            var tag = _unitOfWork.Tags.GetByIdAsync(TagId);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {TagId} not found.");
            }
            await _unitOfWork.Tags.DeleteAsync(tag);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            var tag = await _unitOfWork.Tags.GetAllAsync();
            if (!tag.Any())
            {
                throw new KeyNotFoundException("No Tags found.");
            }

            return tag;

        }

        public async Task<Tag> GetTagAsync(int TagId)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(TagId);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {TagId} not found");
            }
            return tag;
        }

        public async Task UpdateTagAsync(int TagId, TagDTO dto)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(TagId);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {TagId} not found");
            }
            if (!string.IsNullOrWhiteSpace(dto.TagName))
            {
                tag.TagName = dto.TagName;
            }
            if (!string.IsNullOrWhiteSpace(dto.Note))
            {
                tag.Note = dto.Note;
            }
            await _unitOfWork.Tags.UpdateAsync(tag);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
