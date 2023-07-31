using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;

namespace TaskManagementSystem.BLL.Interfaces
{
    public interface IAuthenticationsService
    {
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
        Task<Person> GetPersonByEmail(string email, int id = 0);
        Task<Person> GetPersonByUsername(string username, int id = 0); 
        Task<Person> GetPersonById(int id);

        Task<Person> ChangePassword(int id, UpdatePasswordRequest request);


    }
}
