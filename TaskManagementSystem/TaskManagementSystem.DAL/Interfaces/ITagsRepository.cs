using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL.Entities;

namespace TaskManagementSystem.DAL.Interfaces
{
    public interface ITagsRepository
    {
        Task<Tag> AddTag(Tag entity);
        Task<List<Tag>> GetTags();
        Task<Tag> GetTagById(int id);
        Task<Tag> GetTagByName(string name);
        Task<Tag> DeleteTag(Tag entity);
    }
}
