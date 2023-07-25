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
        /// <summary>
        /// Krijon nje task te ri.
        /// </summary>
        /// <param name="entity">Task-u qe do te krijohet.</param>
        /// <returns>Task-un e krijuar.</returns>
        public async Task<Entities.Task> AddTask(Entities.Task entity)
        {

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Fshin nje task.
        /// </summary>
        /// <param name="entity">Task-u qe duhet fshire.</param>
        /// <returns>Task-un e fshire.</returns>
        public async Task<Entities.Task> DeleteTask(Entities.Task entity)
        {

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Merr nje task ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe identifikon task-un qe duhet marre.</param>
        /// <returns>Task-un perkates.</returns>
        public async Task<Entities.Task> GetTaskById(int id)
        {

            return await _dbSet.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Merr te gjitha task-et.
        /// </summary>
        /// <param name="id">Id qe identikifon perdoruesin e loguar.</param>
        /// <returns>Nese perdoruesi i loguar eshte developer kthen task-et qe i perkasin ketij develoepr-i,
        /// perndryshe kthen te gjitha task-et.<returns>
        public Task<List<Entities.Task>> GetTasks(int id)
        {
            if (id == 0)
            {
                return _dbSet.Where(x => x.IsDeleted != true && x.FinalStatus == false).OrderBy(x => x.EndDate).ToListAsync();
            }
            else
            {
                return _dbSet.Where(x => x.IsDeleted != true && x.FinalStatus == false && x.DeveloperId==id).OrderBy(x => x.EndDate).ToListAsync();
            }

        }
        /// <summary>
        /// Merr te gjitha task-et qe rezutojne te perfunduara.
        /// </summary>
        /// <returns>Kthen listen e task-eve te perfunduara.</returns>
        public Task<List<Entities.Task>> GetCompletedTasks()
        {

            return _dbSet.Where(x => x.IsDeleted != true && x.FinalStatus == true).ToListAsync();
        }
        /// <summary>
        /// Merr te gjitha task-et qe i perkasin developer-it me nje username te caktuar.
        /// </summary>
        /// <param name="developerId">Id qe identifikon developer-in.<param>
        /// <returns>Listen e te gjitha taske-ve te developer-it perkates.</returns>
        public Task<List<Entities.Task>> GetTasksByDeveloperId(int developerId)
        {

            return _dbSet.Where(x => x.IsDeleted != true && x.DeveloperId == developerId && x.FinalStatus == false).OrderBy(x => x.EndDate).ThenByDescending(x => x.Importance).ToListAsync();
        }
        /// <summary>
        /// Merr task-et qe i perkasin nje projekti me nje emer te caktuar.
        /// </summary>
        /// <param name="id">Id qe sherben per te identfikuar projektin.</param>
        /// <returns>Listen e task-eve te projektit perkates.</returns>
        public Task<List<Entities.Task>> GetTasksByProjectId(int id)
        {

            return _dbSet.Where(x => x.IsDeleted != true && x.FinalStatus == false && x.ProjectId == id).ToListAsync();
        }

        /// <summary>
        /// Perditeson te dhenat e nje task-u.
        /// </summary>
        /// <param name="entity">task-u qe do te perditesohet.</param>
        /// <returns>Task-un e perditesuar.</returns>
        public async Task<Entities.Task> UpdateTask(Entities.Task entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Merr te gjithe task-et qe u ka mbetur edhe 3 dite per tu perfunduar.
        /// </summary>
        /// <returns>Listen e task-eve te projektit perkates.</returns>
        public async Task<List<Entities.Task>> GetTasksCloseToDeadline()
        {
            return await _dbSet.Where(x => x.IsDeleted != true && x.PendingStatus == false && x.EndDate <= DateTime.Now.AddDays(3)).OrderBy(x => x.EndDate).ThenByDescending(x => x.Importance).ToListAsync();

        }
    }
}
