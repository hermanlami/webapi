using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.DateValidation;

namespace TaskManagementSystem.DAL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [GreaterThanOrEqualToCurrentDate]
        public DateTime EndDate { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public int ProjectManagerId { get; set; }
        [ForeignKey("ProjectManagerId")]
        public ProjectManager ProjectManager { get; set; } 
    }
}
