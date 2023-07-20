using Microsoft.AspNetCore.Mvc;
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
        Task<List<DTO.Task>> GetTasks(int id, string userRole);
        Task<List<DTO.Task>> GetCompletedTasks();
        Task<DTO.Task> GetTaskById(int id);
        Task<List<DTO.Task>> GetTasksByDevelopersUsername(string username);
        Task<List<DTO.Task>> GetTasksByProjectName(string name);
        Task<List<DTO.Task>> GetTasksByTagName(string name);
        Task<DTO.Task> SetTaskStatus(int id, string userRole, string response=null);
        Task<DTO.Task> UpdateTask(int id, DTO.Task model);
        Task<DTO.Task> DeleteTask(int id);
        Task NotifyForTasksCloseToDeadline();

    }
}
