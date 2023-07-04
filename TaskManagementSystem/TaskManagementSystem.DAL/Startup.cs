using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.DAL.Interfaces;
using TaskManagementSystem.DAL.Repositories;

namespace TaskManagementSystem.DAL
{
    public static class Startup
    {
        public static void RegisterDalServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ITasksRepository, TasksRepository>();
            services.AddScoped<ITagsRepository, TagsRepository>();
            services.AddScoped<ITaskTagsRepository, TaskTagsRepository>();
            services.AddScoped<IDevelopersRepository, DevelopersRepository>();
            services.AddScoped<IProjectManagersRepository, ProjectManagersRepository>();
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
        }
    }
}
