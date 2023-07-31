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
    internal class TagsRepository : ITagsRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Tag> _dbSet;
        public TagsRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Tags;
        }
        /// <summary>
        /// Krijon nje tag te ri.
        /// </summary>
        /// <param name="entity">Tag-u qe do te krijohet</param>
        /// <returns>Tag-un e krijuar.</returns>
        public async Task<Tag> AddTag(Tag entity)
        {

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Fshin nje tag.
        /// </summary>
        /// <param name="entity">Tagu-u qe duhet fshire.</param>
        /// <returns>Tag-un e fshire.</returns>
        public async Task<Tag> DeleteTag(Tag entity)
        {

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Merr nje tag ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar tag-un.</param>
        /// <returns>Tag-un perkates.</returns>
        //public async Task<Tag> GetTagById(int id)
        //{

        //    return await _dbSet.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefaultAsync();
        //}
        /// <summary>
        /// Merr tag-un na bze te emrit te tij.
        /// </summary>
        /// <param name="name">Emri qe sherben per te identifkuar tagun.</param>
        /// <returns>Tag-un perkates.</returns>
        public async Task<Tag> GetTagByName(string name)
        {

            return await _dbSet.Where(x => x.IsDeleted != true && x.Name == name).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Merr te githe tag-et.
        /// </summary>
        /// <returns>Listen e te gjitha tag-eve.</returns>
        public async Task<List<Tag>> GetTags()
        {

            return await _dbSet.Where(x => x.IsDeleted != true).ToListAsync();
        }
    }
}

