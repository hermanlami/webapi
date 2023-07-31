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
        Task<List<DTO.Task>> GetTasks(string userRole, int userId);
        Task<List<DTO.Task>> GetCompletedTasks(string userRole, int userId);
        Task<DTO.Task> GetTaskByName(string name, string userRole="", int userId=0);

        Task<List<DTO.Task>> GetTasksByDevelopersUsername(string username, string userRole, int userId);
        Task<List<DTO.Task>> GetTasksByProjectName(string name, string userRole,int id);
        Task<List<DTO.Task>> GetTasksByTagName(string name, string userRole, int userId);
        Task<DTO.Task> SetTaskStatus(string name, string userRole, int userId, string response=null);
        Task<DTO.Task> UpdateTask(string name, DTO.Task model, int userId, string userRole);
        Task<DTO.Task> DeleteTask(string name);
        Task NotifyForTasksCloseToDeadline();

    }
}
