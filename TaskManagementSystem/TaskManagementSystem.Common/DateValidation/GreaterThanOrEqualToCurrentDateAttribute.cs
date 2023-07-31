using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Common.DateValidation
{
    public class GreaterThanOrEqualToCurrentDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime endDate)
            {
                return endDate >= DateTime.Now;
            }

            return false;
        }
    }
}
