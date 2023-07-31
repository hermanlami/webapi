using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;

namespace TaskManagementSystem.BLL.Interfaces
{
    public interface IProjectsService
    {
        Task<Project> AddProject(Project model);
        Task<List<Project>> GetProjects(int userId, string userRole = "");
       // Task<Project> GetProjectById(int id);
        Task<Project> GetProjectByName(string name, string userRole="", int userId=0);
        Task<Project> UpdateProject(string name, Project model);
        Task<Project> DeleteProject(string name);
    }
}
