using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Common;

namespace TaskManagementSystem.BLL
{
    public static class ServiceExceptionHandler
    {
        public static async Task<T> HandleExceptionAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
