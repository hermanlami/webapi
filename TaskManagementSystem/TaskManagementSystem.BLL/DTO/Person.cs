using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.Enums;

namespace TaskManagementSystem.BLL.DTO
{
    public abstract class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string PasswordHash { get; set; }
        //public byte[] PasswordSalt { get; set; }
        public PersonType PersonType { get; set; }
        public DateTime Birthday { get; set; }
    }
}
