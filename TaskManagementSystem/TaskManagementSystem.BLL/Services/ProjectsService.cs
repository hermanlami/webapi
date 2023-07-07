using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _repository;
        private readonly ILogger<ProjectsService> _logger;
        public ProjectsService(IProjectsRepository repository, ILogger<ProjectsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<Project> AddProject(Project model)
        {
            try
            {
                var dalProject = new DAL.Entities.Project()
                {
                    Name = model.Name,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ProjectManagerId = model.ProjectManagerId,
                };
                var addedProject = await _repository.AddProject(dalProject);
                model.Id=addedProject.Id;
                if (addedProject.Id > 0)
                {
                    _logger.LogInformation("Project added successfully");
                    return model;
                }
                else
                {
                    _logger.LogError("Project could not be added");

                }

            }
            catch (Exception ex)
            {

            }
            return new DTO.Project();
        }

        public async Task<Project> DeleteProject(Project model)
        {
            try
            {
                var project = await _repository.GetProjectById(model.Id);
                if (project != null)
                {
                    project.IsDeleted = true;
                    var deletedProject = await _repository.DeleteProject(project);
                    if (deletedProject != null)
                    {
                        _logger.LogInformation("Project deleted successfully");

                        return new DTO.Project()
                        {
                            Name = deletedProject.Name,
                            StartDate = deletedProject.StartDate,
                            EndDate = deletedProject.EndDate,
                            ProjectManagerId = deletedProject.ProjectManagerId,
                        };

                    }
                    else
                    {
                        _logger.LogError("Project could not be deleted");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Project();
        }

        public async Task<Project> GetProjectById(int id)
        {
            try
            {
                var project = await _repository.GetProjectById(id);
                if (project != null)
                {
                    _logger.LogInformation("Project retrieved successfully");

                    return new DTO.Project()
                    {
                        Id = project.Id,
                        Name = project.Name,
                        StartDate = project.StartDate,
                        EndDate = project.EndDate,
                        ProjectManagerId = project.ProjectManagerId,
                    };
                }
                else
                {
                    _logger.LogError("Project could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Project();
        }

        public async Task<List<Project>> GetProjects()
        {
            try
            {
                var projects = await _repository.GetProjects();
                var dtoProjects = new List<Project>();
                if (projects != null)
                {
                    _logger.LogInformation("Projects retrieved successfully");

                    projects.ForEach(x => dtoProjects.Add(new Project
                    {
                        Id = x.Id,
                        Name = x.Name,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        ProjectManagerId = x.ProjectManagerId,
                    }));
                    return dtoProjects;
                }
                else
                {
                    _logger.LogError("Projects could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new List<Project>();
        }

        public async Task<Project> UpdateProject(Project model)
        {
            try
            {
                var project = await _repository.GetProjectById(model.Id);
                if (project != null)
                {
                    project.Name = model.Name;
                    project.StartDate = model.StartDate;
                    project.EndDate = model.EndDate;
                    project.ProjectManagerId = model.ProjectManagerId;
                    var updated = await _repository.UpdateProject(project);
                    if (updated != null)
                    {
                        _logger.LogInformation("Project updated successfully");

                        return new Project()
                        {
                            Name = updated.Name,
                            StartDate = updated.StartDate,
                            EndDate = updated.EndDate,
                            ProjectManagerId = updated.ProjectManagerId,
                        };
                    }
                    else
                    {
                        _logger.LogError("Project could not be updated");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new Project();
        }
    }
}
