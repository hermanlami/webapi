using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.DAL.Entities
{
    public class Developer : Person
    {
        public int ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public ProjectManager Manager { get; set; }
    }
}
