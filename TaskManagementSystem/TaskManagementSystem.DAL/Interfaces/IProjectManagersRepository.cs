using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL.Entities;

namespace TaskManagementSystem.DAL.Interfaces
{
    public interface IProjectManagersRepository
    {
        Task<ProjectManager> AddProjectManager(ProjectManager entity);
        Task<List<ProjectManager>> GetProjectManagers();
        Task<ProjectManager> GetProjectManagerById(int id);
        Task<ProjectManager> UpdateProjectManager(ProjectManager entity);
        Task<ProjectManager> DeleteProjectManager(ProjectManager entity);
    }
}
