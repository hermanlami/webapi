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
        public async Task<Tag> AddTag(Tag entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Tag> DeleteTag(Tag entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Tag> GetTagById(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Tag>> GetTags()
        {
           return await _dbSet.ToListAsync();
        }

        public async Task<Tag> UpdateTag(Tag entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

