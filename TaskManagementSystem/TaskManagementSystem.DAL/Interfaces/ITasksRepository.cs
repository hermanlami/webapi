using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.DAL.Interfaces
{
    public interface ITasksRepository
    {
        Task<Entities.Task> AddTask(Entities.Task entity);
        Task<List<Entities.Task>> GetTasks();
        Task<Entities.Task> GetTaskById(int id);
        Task<Entities.Task> UpdateTask(Entities.Task entity);
        Task<Entities.Task> DeleteTask(Entities.Task entity);

    }
}
