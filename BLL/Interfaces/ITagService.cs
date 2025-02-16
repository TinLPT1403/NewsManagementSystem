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
        Task CreateTag(TagDTO dto);
        Task UpdateTag(int TagId, TagDTO dto);
        Task DeleteTag(int TagId);
        Task<Tag> GetTag(int TagId);
        Task<List<Tag>> GetAllTags();
    }
}
