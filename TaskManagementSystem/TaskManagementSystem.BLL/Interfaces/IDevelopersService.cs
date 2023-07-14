using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;

namespace TaskManagementSystem.BLL.Interfaces
{
    public interface IDevelopersService
    {
        Task<Developer> AddDeveloper(Developer model);
        Task<List<Developer>> GetDevelopers();
        Task<Developer> GetDeveloperById(int id);
        Task<Developer> UpdateDeveloper(int id, Developer model);
        Task<Developer> DeleteDeveloper(int id);
    }
}
