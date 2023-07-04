using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL.Entities;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.DAL.Repositories
{
    internal class ProjectsRepository : IProjectsRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Project> _dbSet;
        public ProjectsRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Projects;
        }
        public async Task<Project> AddProject(Project entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Project> DeleteProject(Project entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Project> GetProjectById(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Project>> GetProjects()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Project> UpdateProject(Project entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
