using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL.Entities;

namespace TaskManagementSystem.DAL.Interfaces
{
    public interface IAuthenticationsRepository
    {
        Task<Person> GetPersonByEmail(string email,int id=0);
        Task<Person> GetPersonByUsername(string username,int id=0);
        Task<Person> ChangePassword(Person person);
        Task<Person> GetPersonById(int id);


    }
}
