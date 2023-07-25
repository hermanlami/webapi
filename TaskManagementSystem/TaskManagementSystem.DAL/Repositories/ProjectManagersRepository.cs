using Microsoft.EntityFrameworkCore;
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
    internal class ProjectManagersRepository : IProjectManagersRepository
    {
        private readonly AppDbContext _context;
        public ProjectManagersRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Krijon ne menaxher te ri projekti.
        /// </summary>
        /// <param name="entity">Modeli qe sherben per te krijuar menaxherin e ri te projetit.</param>
        /// <returns>Menaxherin e krijuar.</returns>
        public async Task<ProjectManager> AddProjectManager(ProjectManager entity)
        {

            await _context.People.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Fshin nje menaxher projekti.
        /// </summary>
        /// <param name="entity">Menaxheri qe do te fshihet.</param>
        /// <returns>Menaxherin e fshire.</returns>
        public async Task<ProjectManager> DeleteProjectManager(ProjectManager entity)
        {

            _context.People.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Merr nje menaxher projekti ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar menaxherin e projektit.</param>
        /// <returns>Menaxherin perkates.</returns>
        public async Task<ProjectManager> GetProjectManagerById(int id)
        {

            return (ProjectManager) await _context.People.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ProjectManager> GetProjectManagerByEmail(string email)
        {
            return (ProjectManager) await _context.People.Where(x => x.IsDeleted != true && x.PersonType == PersonType.ProjectManager && x.Email == email).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Merr te gjithe menaxheret e projekteve.
        /// </summary>
        /// <returns>Listen e te gjithe menaxhereve.</returns>
        public async Task<List<ProjectManager>> GetProjectManagers()
        {

            return await _context.People.Where(x => x.PersonType == PersonType.ProjectManager && x.IsDeleted != true).Cast<ProjectManager>().ToListAsync();
        }
        /// <summary>
        /// Perditeson menaxherin e projektit.
        /// </summary>
        /// <param name="entity">Modeli ne baze te te cilit do te perditesohen te dhenat.</param>
        /// <returns>Menaxherin e perditesuar.</returns>
        public async Task<ProjectManager> UpdateProjectManager(ProjectManager entity)
        {

            _context.People.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
