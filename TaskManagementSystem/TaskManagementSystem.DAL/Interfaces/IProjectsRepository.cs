using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL.Entities;

namespace TaskManagementSystem.DAL.Interfaces
{
    public interface IProjectsRepository
    {
        Task<Project> AddProject(Project entity);
        Task<List<Project>> GetProjects();
        Task<Project> GetProjectById(int id);
        Task<Project> UpdateProject(Project entity);
        Task<Project> DeleteProject(Project entity);
    }
}
