using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Common.CustomExceptions
{
    public class CustomException : Exception
    {
        public CustomException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
