using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL.Entities;
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

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Entities.Task> DeleteTask(Entities.Task entity)
        {

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Entities.Task> GetTaskById(int id)
        {

            return await _dbSet.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Entities.Task>> GetTasks()
        {

            return _dbSet.Where(x => x.IsDeleted != true && x.Status == false).OrderBy(x => x.EndDate).ToListAsync();


        }
        public Task<List<Entities.Task>> GetCompletedTasks()
        {

            return _dbSet.Where(x => x.IsDeleted != true && x.Status == true).ToListAsync();
        }

        public Task<List<Entities.Task>> GetTasksByDeveloperId(int developerId)
        {

            return _dbSet.Where(x => x.IsDeleted != true && x.DeveloperId == developerId && x.Status == false).OrderBy(x => x.EndDate).ThenByDescending(x => x.Importance).ToListAsync();
        }

        public Task<List<Entities.Task>> GetTasksByProjectId(int id)
        {

            return _dbSet.Where(x => x.IsDeleted != true && x.Status == false && x.ProjectId == id).ToListAsync();
        }

        public async Task<Entities.Task> UpdateTask(Entities.Task entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Entities.Task>> GetTasksCloseToDeadline()
        {
            return await _dbSet.Where(x => x.IsDeleted != true && x.Status == false && x.EndDate <= DateTime.Now.AddDays(3)).OrderBy(x => x.EndDate).ThenByDescending(x => x.Importance).ToListAsync();

        }
    }
}
