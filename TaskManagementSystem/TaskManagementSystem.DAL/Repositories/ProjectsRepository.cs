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
            using (_context)
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<Project> DeleteProject(Project entity)
        {
            using (_context)
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<Project> GetProjectById(int id)
        {
            using (_context)
            {
                return await _dbSet.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<Project> GetProjectByName(string name)
        {
            using (_context)
            {
                return await _dbSet.Where(x => x.IsDeleted != true && x.Name == name).FirstOrDefaultAsync();
            }
        }

        public async Task<List<Project>> GetProjects()
        {
            using (_context)
            {
                return await _dbSet.Where(x => x.IsDeleted != true).ToListAsync();
            }
        }

        public async Task<Project> UpdateProject(Project entity)
        {
            using (_context)
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }
    }
}
