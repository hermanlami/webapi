﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.BLL.DTO
{
    public class Developer : Person
    {
        public int ManagerId { get; set; }

    }
}
