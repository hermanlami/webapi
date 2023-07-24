using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Common.CustomExceptions
{
    public class DuplicateInputException : Exception
    {
        public DuplicateInputException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
