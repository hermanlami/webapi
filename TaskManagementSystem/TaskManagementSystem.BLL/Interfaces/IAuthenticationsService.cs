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
        Task<Person> GetPersonByEmail(string email);
        Task<Person> GetPersonByUsername(string username);
        Task<Person> GetPersonById(int id);

        Task<Person> ChangePassword(int id, UpdatePasswordRequest request);


    }
}
