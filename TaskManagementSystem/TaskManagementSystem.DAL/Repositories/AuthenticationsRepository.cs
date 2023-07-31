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
        /// <summary>
        /// Ndryshon password-in e perdoruesit kur ai mund te jape sakte password-in e vjeter.
        /// </summary>
        /// <param name="person">Modeli qe permban passwordin e ri</param>
        /// <returns>Perdoruesin me password-in e perditesuar.</returns>
        public async Task<Person> ChangePassword(Person person)
        {
            _dbSet.Update(person);
            await _context.SaveChangesAsync();
            return person;
        }
        /// <summary>
        /// Merr nje person ne baze te adreses se tij email.
        /// </summary>
        /// <param name="email">Email qe sherben per te identifikuar personin.</param>
        /// <returns>Personin perkates.</returns>
        public async Task<Person> GetPersonByEmail(string email, int id)
        {
            return id==0? await _dbSet.Where(x => x.IsDeleted != true && x.Email == email).FirstOrDefaultAsync(): await _dbSet.Where(x => x.IsDeleted != true && x.Email == email && x.Id != id).FirstOrDefaultAsync(); 
        }
        /// <summary>
        /// Merr nje person ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar personin.</param>
        /// <returns>Personin perkates.</returns>
        public async Task<Person> GetPersonById(int id)
        {
            return await _dbSet.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Merr nje person ne baze te username-it te tij.
        /// </summary>
        /// <param name="username">Username qe sherben per te identifikuar personin.</param>
        /// <returns>Personin perkates.</returns>
        public async Task<Person> GetPersonByUsername(string username, int id)
        {
            return id == 0 ? await _dbSet.Where(x => x.IsDeleted != true && x.Username == username).FirstOrDefaultAsync(): await _dbSet.Where(x => x.IsDeleted != true && x.Username == username&&x.Id!=id).FirstOrDefaultAsync();
            
        }
    }
}
