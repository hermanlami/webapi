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
    internal class TaskTagsRepository : ITaskTagsRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TaskTag> _dbSet;
        public TaskTagsRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.TaskTags;
        }
        /// <summary>
        /// Shton nje lidhje te re mes nje task-u dhe nje tag-u.
        /// </summary>
        /// <param name="entity">Modeli qe do te shtohet.</param>
        /// <returns>Task-tagun e ri.</returns>
        public async Task<TaskTag> AddTaskTag(TaskTag entity)
        {

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Merr nje task-tag ne baze te Id se tag.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar tag-un.</param>
        /// <returns>Task-tagun perkates.</returns>
        public async Task<List<TaskTag>> GetTaskTagByTagId(int id)
        {

            return await _dbSet.Where(x => x.TagId == id).ToListAsync();
        }

    }
}
