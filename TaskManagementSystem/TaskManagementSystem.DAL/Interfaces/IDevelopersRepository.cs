using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL.Entities;

namespace TaskManagementSystem.DAL.Interfaces
{
    public interface IDevelopersRepository
    {
        Task<Developer> AddDeveloper(Developer entity);
        Task<List<Developer>> GetDevelopers();
        Task<Developer> GetDeveloperByEmail(string email);

        Task<Developer> GetDeveloperById(int id);
        Task<Developer> UpdateDeveloper(Developer entity);
        Task<Developer> DeleteDeveloper(Developer entity);
    }
}
