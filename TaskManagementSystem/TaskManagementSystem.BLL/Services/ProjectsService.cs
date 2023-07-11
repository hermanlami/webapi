using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.DAL.Interfaces;

[assembly: InternalsVisibleTo("TaskManagementSystem.UnitTesting")]

namespace TaskManagementSystem.BLL.Services
{
    internal class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _repository;
        private readonly ILogger<ProjectsService> _logger;
        private readonly IMapper _mapper;
        public ProjectsService(IProjectsRepository repository, ILogger<ProjectsService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public ProjectsService(IProjectsRepository repository)
        {
            _repository = repository;
        }
        public async Task<Project> AddProject(Project model)
        {
            try
            {
                var dalProject = _mapper.Map<DAL.Entities.Project>(model);

                var addedProject = await _repository.AddProject(dalProject);

                if (addedProject.Id > 0)
                {
                    _logger.LogInformation("Project added successfully");
                    return _mapper.Map<DTO.Project>(addedProject);
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

                        return _mapper.Map<DTO.Project>(deletedProject);

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

                    return _mapper.Map<DTO.Project>(project);

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
                if (projects != null)
                {
                    _logger.LogInformation("Projects retrieved successfully");

                    return _mapper.Map<List<DTO.Project>>(projects);
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
                    model.Id = project.Id;
                    project.Name = model.Name;
                    project.StartDate = model.StartDate;
                    project.EndDate = model.EndDate;
                    project.ProjectManagerId = model.ProjectManagerId;
                    var updated = await _repository.UpdateProject(project);
                    if (updated != null)
                    {
                        _logger.LogInformation("Project updated successfully");

                        return _mapper.Map<DTO.Project>(updated);
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
