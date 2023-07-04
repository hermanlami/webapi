using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.DAL.Entities
{
    public class TaskTag
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task Task { get; set; }
        public int TagId { get; set; }
        [ForeignKey("TagId")]
        public Tag Tag { get; set; }

    }
}
