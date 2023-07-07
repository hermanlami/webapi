﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common.Enums;

namespace TaskManagementSystem.BLL.DTO
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public Importance Importance { get; set; }
        public int ProjectId { get; set; }
        public string Tags { get; set; }
    }
}