using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL.Entities;

namespace TaskManagementSystem.DAL.Interfaces
{
    public interface ITaskTagsRepository
    {
        Task<TaskTag> AddTaskTag(TaskTag entity);
        Task<List<TaskTag>> GetTaskTags();
        Task<TaskTag> GetTaskTagById(int id);
        Task<List<TaskTag>> GetTaskTagByTagId(int id);

        Task<TaskTag> UpdateTaskTag(TaskTag entity);
        Task<TaskTag> DeleteTaskTag(TaskTag entity);
    }
}
