using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;

namespace TaskManagementSystem.BLL.Interfaces
{
    public interface ITaskTagsService
    {
        Task<TaskTag> AddTaskTag(TaskTag model);
        Task<List<TaskTag>> GetTaskTagByTagId(int id);

    }
}
