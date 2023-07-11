using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.DAL;

namespace TaskManagementSystem.BLL
{
    public static class Startup
    {
        public static void RegisterBllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDalServices(configuration.GetConnectionString("DefaultConnection"));
            services.AddScoped<ITagsService, TagsService>();
            services.AddScoped<ITasksService, TasksService>();
            services.AddScoped<ITaskTagsService, TaskTagsService>();
            services.AddScoped<IDevelopersService, DevelopersService>();
            services.AddScoped<IProjectManagersService, ProjectManagersService>();
            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddScoped<ITokensService, TokensService>();
        }
    }
}
