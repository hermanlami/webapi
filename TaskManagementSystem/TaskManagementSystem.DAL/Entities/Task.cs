using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.DateValidation;
using TaskManagementSystem.Common.Enums;

namespace TaskManagementSystem.DAL.Entities
{
    public class Task
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        [Required]
        [GreaterThanOrEqualToCurrentDate]
        public DateTime EndDate { get; set; }

        [Required]
        public bool PendingStatus { get; set; }
        [Required]

        public bool FinalStatus { get; set; }
        [Required]

        public Importance Importance { get; set; }
        public bool IsDeleted { get; set; }
        [Required]

        public int DeveloperId { get; set; }                                   
        public Developer Developer { get; set; }
        [Required]

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}
