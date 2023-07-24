using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Common.CustomExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
