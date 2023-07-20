using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// Krijon nje developer te ri.
        /// </summary>
        /// <param name="entity">Modeli qe sherben per te krijuar developer-in e ri.</param>
        /// <returns>Developerin e krijuar.</returns>
        public async Task<Developer> AddDeveloper(Developer entity)
        {

                await _context.People.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
           
        }
        /// <summary>
        /// Fshin nje developer.
        /// </summary>
        /// <param name="entity">Entiteti qe duhet fshire.</param>
        /// <returns>Developer-in e fshire.</returns>
        public async Task<Developer> DeleteDeveloper(Developer entity)
        {

            _context.People.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Developer> GetDeveloperByEmail(string email)
        {

            return (Developer)await _context.People.Where(x => x.IsDeleted != true && x.PersonType == PersonType.Developer && x.Email == email).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Kap nje develoepr ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe sherben per te idetifikuar developer-in qe duhet fshire.</param>
        /// <returns>Developer-in perkates.</returns>
        public async Task<Developer> GetDeveloperById(int id)
        {

            return (Developer)await _context.People.Where(x => x.IsDeleted != true && x.PersonType==PersonType.Developer && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Developer> GetDeveloperByUsername(string username)
        {

            return (Developer)await _context.People.Where(x => x.IsDeleted != true && x.PersonType == PersonType.Developer && x.Username == username).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Merr te gjithe developers.
        /// </summary>
        /// <returns>Listen e te gjithe developers.</returns>
        public async Task<List<Developer>> GetDevelopers()
        {

            return await _context.People.Where(x => x.PersonType == PersonType.Developer && x.IsDeleted != true).Cast<Developer>().ToListAsync();
        }
        /// <summary>
        /// Perditeson te dhenat e nje developer.
        /// </summary>
        /// <param name="entity">Modeli ne baze te te cilit behet perditesimi i te dhenave.</param>
        /// <returns>Developer-in e perditesuar.</returns>
        public async Task<Developer> UpdateDeveloper(Developer entity)
        {

            _context.People.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

    }
}
