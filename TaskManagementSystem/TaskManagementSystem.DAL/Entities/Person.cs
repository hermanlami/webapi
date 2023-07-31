using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.DateValidation;
using TaskManagementSystem.Common.Enums;

namespace TaskManagementSystem.DAL.Entities
{
    public abstract class Person
    {
        public int Id { get; set; }
        [Required]
        [MaxLength]
        public string FirstName { get; set; }
        [Required]
        [MaxLength]
        public string LastName { get; set; }
        [Required]
        public PersonType PersonType { get; set; }
        [Required]
        [GreaterThanOrEqualTo18]
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
