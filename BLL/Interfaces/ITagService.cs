using BLL.DTOs;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITagService
    {
        Task CreateTagAsync(TagDTO dto);
        Task UpdateTagAsync(int TagId, TagDTO dto);
        Task DeleteTagAsync(int TagId);
        Task<Tag> GetTagAsync(int TagId);
        Task<IEnumerable<Tag>> GetAllTagsAsync();

    }
}
