using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Common.DateValidation
{
    public class GreaterThanOrEqualTo18 : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime birthday)
            {
                return (DateTime.Now-birthday).TotalDays/365>=18;
            }

            return false;
        }
    }
}
