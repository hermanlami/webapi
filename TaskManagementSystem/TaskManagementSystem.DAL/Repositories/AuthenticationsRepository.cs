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
    internal class AuthenticationsRepository : IAuthenticationsRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Person> _dbSet;
        public AuthenticationsRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.People;
        }

        public async Task<Person> ChangePassword(Person person)
        {
            _dbSet.Update(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            return await _dbSet.Where(x => x.IsDeleted != true && x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await _dbSet.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Person> GetPersonByUsername(string username)
        {
            return await _dbSet.Where(x => x.IsDeleted != true && x.Username == username).FirstOrDefaultAsync();

        }
    }
}
