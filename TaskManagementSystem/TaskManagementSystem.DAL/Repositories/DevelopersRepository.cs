﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    internal class DevelopersRepository : IDevelopersRepository
    {
        private readonly AppDbContext _context;
        public DevelopersRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Developer> AddDeveloper(Developer entity)
        {
            using (_context)
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<Developer> DeleteDeveloper(Developer entity)
        {
            using (_context)
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<Developer> GetDeveloperByEmail(string email)
        {
            using (_context)
            {
                return (Developer)await _context.People.Where(x => x.IsDeleted != true && x.Email == email).FirstOrDefaultAsync();
            }
        }

        public async Task<Developer> GetDeveloperById(int id)
        {
            using (_context)
            {
                return (Developer)await _context.People.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<Developer> GetDeveloperByUsername(string username)
        {
            using (_context)
            {
                return (Developer)await _context.People.Where(x => x.IsDeleted != true && x.Username == username).FirstOrDefaultAsync();
            }
        }

        public async Task<List<Developer>> GetDevelopers()
        {
            using (_context)
            {
                return await _context.People.Where(x => x.PersonType == PersonType.Developer && x.IsDeleted != true).Cast<Developer>().ToListAsync();
            }
        }

        public async Task<Developer> UpdateDeveloper(Developer entity)
        {
            using (_context)
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }
    }
}
