using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Common.DateValidation;
using TaskManagementSystem.Common.Enums;

namespace TaskManagementSystem.BLL.DTO
{
    public class Task
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task name is required.")]
        [MaxLength(50, ErrorMessage = "Task name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [Display(Name = "End Date")]
        [GreaterThanOrEqualToCurrentDate(ErrorMessage = "End date must be greater than or equal to the current date.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Pending status is required.")]
        [Display(Name = "Pending Status")]

        public bool PendingStatus { get; set; }

        [Required(ErrorMessage = "Final status is required.")]
        [Display(Name = "Final Status")]

        public bool FinalStatus { get; set; }

        [Required(ErrorMessage = "Importance is required.")]
        public Importance Importance { get; set; }

        [Required(ErrorMessage = "Project ID is required.")]
        [Display(Name = "Project Id")]

        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Developer ID is required.")]
        [Display(Name = "Developer Id")]

        public int DeveloperId { get; set; }

        public string Developer { get; set; }

        [MaxLength(200, ErrorMessage = "Tags cannot exceed 200 characters.")]
        public string Tags { get; set; }
    }
}
