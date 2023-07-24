using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Common.CustomExceptions
{
    public class FailedToAuthencitcateException : Exception
    {
        public FailedToAuthencitcateException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
