using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;

namespace TaskManagementSystem.BLL.Interfaces
{
    public interface ITokensService
    {
        TokenResponse CreateToken(DAL.Entities.Person user);
        Task<TokenResponse> RefreshToken(string refreshToken);


    }
}
