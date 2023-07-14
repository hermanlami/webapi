using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;

namespace TaskManagementSystem.BLL.Interfaces
{
    public interface IProjectManagersService
    {
        Task<ProjectManager> AddProjectManager(ProjectManager model);
        Task<List<ProjectManager>> GetProjectManagers();
        Task<ProjectManager> GetProjectManagerById(int id);
        Task<ProjectManager> UpdateProjectManager(int id, ProjectManager model);
        Task<ProjectManager> DeleteProjectManager(int id);
    }
}
