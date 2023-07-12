using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.DAL.Repositories
{
    internal class TasksRepository : ITasksRepository
    {
        private readonly DbSet<Entities.Task> _dbSet;
        private readonly AppDbContext _context;

        public TasksRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Tasks;
        }
        public async Task<Entities.Task> AddTask(Entities.Task entity)
        {
            using (_context)
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<Entities.Task> DeleteTask(Entities.Task entity)
        {
            using (_context)
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<Entities.Task> GetTaskById(int id)
        {
            using (_context)
            {
                return await _dbSet.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefaultAsync();
            }
        }

        public Task<List<Entities.Task>> GetTasks()
        {
            using (_context)
            {
                return _dbSet.Where(x => x.IsDeleted != true&& x.Status==false).OrderBy(x => x.EndDate).ToListAsync();
            }
        }
        public Task<List<Entities.Task>> GetCompletedTasks()
        {
            using (_context)
            {
                return _dbSet.Where(x => x.IsDeleted != true && x.Status == true).ToListAsync();
            }
        }

        public Task<List<Entities.Task>> GetTasksByDeveloperId(int developerId)
        {
            using (_context)
            {
                return _dbSet.Where(x => x.IsDeleted != true && x.DeveloperId == developerId && x.Status == false).OrderBy(x => x.EndDate).ThenByDescending(x => x.Importance).ToListAsync();
            }
        }

        public Task<List<Entities.Task>> GetTasksByProjectId(int id)
        {
            using (_context)
            {
                return _dbSet.Where(x => x.IsDeleted != true && x.Status == false && x.ProjectId == id).ToListAsync();
            }
        }

        public async Task<Entities.Task> UpdateTask(Entities.Task entity)
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
