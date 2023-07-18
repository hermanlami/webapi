using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.BLL.DTO
{
    public class AuthenticationResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public  string RefreshToken { get; set; }
    }
}
