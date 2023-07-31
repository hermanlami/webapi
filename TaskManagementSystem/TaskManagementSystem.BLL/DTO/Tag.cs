using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.BLL.DTO
{
    public class Tag
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tag name is required.")]
        [MaxLength(50, ErrorMessage = "Tag name cannot exceed 50 characters.")]
        public string Name { get; set; }

    }
}
