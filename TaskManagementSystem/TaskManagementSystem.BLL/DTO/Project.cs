using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.BLL;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.DateValidation;

namespace TaskManagementSystem.BLL.DTO
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        [MaxLength(50, ErrorMessage = "Project name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [Display(Name = "End Date")]
        [GreaterThanOrEqualToCurrentDate(ErrorMessage = "End date must be greater than or equal to the start date.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Project manager ID is required.")]
        public int ProjectManagerId { get; set; }
        [Display(Name = "Project Manager")]
        public string ProjectManager { get; set; }
    }
}
