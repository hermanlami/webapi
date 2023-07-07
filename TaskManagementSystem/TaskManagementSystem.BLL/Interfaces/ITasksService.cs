using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.BLL.Interfaces
{
    public interface ITasksService
    {
        Task<DTO.Task> AddTask(DTO.Task model);
        Task<List<DTO.Task>> GetTasks();
        Task<DTO.Task> GetTaskById(int id);
        //Task<List<DTO.Task>> GetTasksByDeveloperId(int developerId);
        Task<DTO.Task> UpdateTask(DTO.Task model);
        Task<DTO.Task> DeleteTask(DTO.Task model);
    }
}
