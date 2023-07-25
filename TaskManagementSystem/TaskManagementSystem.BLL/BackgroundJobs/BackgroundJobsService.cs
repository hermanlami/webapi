using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.BackgroundJobs
{
    public class BackgroundJobsService : BackgroundService
    {
        private readonly ITasksService _tasksService;
        public BackgroundJobsService(ITasksService tasksService)
        {
            _tasksService = tasksService;   
        }

        /// <summary>
        /// Ekzekuton cdo dite ne oren 9 metoden qe dergon email-e per tasket qe mbarojne se shpejti.
        /// </summary>
        /// <param name="stoppingToken">Token qe ndalon background service.</param>
        protected async override Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime now = DateTime.UtcNow;

                DateTime nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
                if (now >= nextExecutionTime)
                {
                    nextExecutionTime = nextExecutionTime.AddDays(1);
                }
                TimeSpan timeUntilNextExecution = nextExecutionTime - now;

                await Task.Delay(timeUntilNextExecution, stoppingToken);

                await _tasksService.NotifyForTasksCloseToDeadline();

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); 
            }
        }
    }
}
