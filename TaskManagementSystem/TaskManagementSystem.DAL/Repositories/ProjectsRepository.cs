using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DAL.Entities;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.DAL.Repositories
{
    internal class ProjectsRepository : IProjectsRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Project> _dbSet;
        public ProjectsRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Projects;
        }
        /// <summary>
        /// Krijon nje project te ri.
        /// </summary>
        /// <param name="entity"> Modeli qe duhet te krijohet.</param>
        /// <returns>Projektin e krijuar.</returns>
        public async Task<Project> AddProject(Project entity)
        {
            await _context.Projects.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Fshin nje projekt.
        /// </summary>
        /// <param name="entity">Projekti qe duhet fshire.</param>
        /// <returns>Projektin e fshire.</returns>
        public async Task<Project> DeleteProject(Project entity)
        {

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Merr nje projekt ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe identifikon projektin qe duhet marre.</param>
        /// <returns>Projektin perkates.</returns>

        //public async Task<Project> GetProjectById(int id)
        //{

        //    return await _dbSet.Where(x => x.IsDeleted != true && x.Id == id).Include(x => x.ProjectManager).FirstOrDefaultAsync();
        //}

        /// <summary>
        /// Merr nje projekt bazuar ne emrin e tij.
        /// </summary>
        /// <param name="name">Emri qe sherben per te identifikuar projektin.</param>
        /// <returns>Projektin perkates.</returns>
        public async Task<Project> GetProjectByName(string name, int id, int userId)
        {
            if (id == 0 && userId == 0)
            {
                return await _dbSet.Where(x => x.IsDeleted != true && x.Name == name).Include(x => x.ProjectManager).FirstOrDefaultAsync();
            }
            if (id==0&& userId!=0)
            {
                return await _dbSet.Where(x => x.IsDeleted != true && x.Name == name && x.ProjectManagerId==userId).Include(x => x.ProjectManager).FirstOrDefaultAsync();
            }
            if (id!=0)
            {
                return await _dbSet.Where(x => x.IsDeleted != true && x.Name == name && x.Id != id).Include(x => x.ProjectManager).FirstOrDefaultAsync();
            }
            return null;
        }
        /// <summary>
        /// Merr te gjitha projektet.
        /// </summary>
        /// <returns>Listen e te gjitha projekteve.</returns>
        public async Task<List<Project>> GetProjects(int userId)
        {

            return userId==0? await _dbSet.Where(x => x.IsDeleted != true).Include(x => x.ProjectManager).ToListAsync(): await _dbSet.Where(x => x.IsDeleted != true&& x.ProjectManagerId==userId).Include(x => x.ProjectManager).ToListAsync();
        }
        /// <summary>
        /// Perditeson nje projekt.
        /// </summary>
        /// <param name="entity">Projekti qe duhet perditesuar</param>
        /// <returns>Projektin e perditesuar.</returns>
        public async Task<Project> UpdateProject(Project entity)
        {

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
