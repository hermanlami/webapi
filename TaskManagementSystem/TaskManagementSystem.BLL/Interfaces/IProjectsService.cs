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
        Task<List<Project>> GetProjects();
        Task<Project> GetProjectById(int id);
        Task<Project> UpdateProject(Project model);
        Task<Project> DeleteProject(Project model);
    }
}
