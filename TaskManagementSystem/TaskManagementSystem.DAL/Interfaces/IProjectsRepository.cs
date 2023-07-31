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
        Task<List<Project>> GetProjects(int userId=0);
      //  Task<Project> GetProjectById(int id);
        Task<Project> GetProjectByName(string name,int id=0, int userId=0);
        Task<Project> UpdateProject(Project entity);
        Task<Project> DeleteProject(Project entity);
    }
}
