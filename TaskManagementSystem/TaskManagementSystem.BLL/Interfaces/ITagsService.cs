using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;

namespace TaskManagementSystem.BLL.Interfaces
{
    public interface ITagsService
    {
        Task<Tag> AddTag(Tag model);
        Task<List<Tag>> GetTags();
        //Task<Tag> GetTagById(int id);
        Task<Tag> GetTagByName(string name);

        Task<Tag> DeleteTag(string name);
    }
}
