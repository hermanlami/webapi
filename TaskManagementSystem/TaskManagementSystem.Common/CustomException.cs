using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Common
{
    public class CustomException:Exception
    {
        public CustomException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
