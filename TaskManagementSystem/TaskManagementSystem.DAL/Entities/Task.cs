﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.Enums;

namespace TaskManagementSystem.DAL.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public bool PendingStatus { get; set; }
        public bool FinalStatus { get; set; }
        public Importance Importance { get; set; }
        public bool IsDeleted { get; set; }
        public int DeveloperId { get; set; }                                   
        public Developer Developer { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}
