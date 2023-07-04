using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.Enums;
using TaskManagementSystem.DAL.Entities;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.DAL.Repositories
{
    internal class ProjectManagersRepository : IProjectManagersRepository
    {
        private readonly AppDbContext _context;
        public ProjectManagersRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ProjectManager> AddProjectManager(ProjectManager entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ProjectManager> DeleteProjectManager(ProjectManager entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ProjectManager> GetProjectManagerById(int id)
        {
            return (ProjectManager)await _context.People.FirstOrDefaultAsync(x => x.Id==id);
        }

        public async Task<List<ProjectManager>> GetProjectManagers()
        {
            return await _context.People.Where(x=>x.PersonType==PersonType.ProjectManager).Cast<ProjectManager>().ToListAsync();
        }

        public async Task<ProjectManager> UpdateProjectManager(ProjectManager entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
