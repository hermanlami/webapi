using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DAL;

namespace TaskManagementSystem.BLL
{
    public static class Startup
    {
        public static void RegisterBllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDalServices(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
