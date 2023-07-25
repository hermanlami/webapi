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
        /// <summary>
        /// Trajton exceptions qe hidhen gjate ekzekutimit te action.
        /// </summary>
        /// <typeparam name="T">Tipi i kthimit te action.</typeparam>
        /// <param name="action">Metoda qe do te ekzekutohet.</param>
        /// <returns>Exception qe ka ndodhur gjate ekzekutimit te action.</returns>
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

        public static async Task HandleExceptionAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
