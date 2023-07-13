using AutoMapper;
using Serilog;
using System.Runtime.CompilerServices;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common;
using TaskManagementSystem.DAL.Interfaces;

[assembly: InternalsVisibleTo("TaskManagementSystem.UnitTesting")]

namespace TaskManagementSystem.BLL.Services
{
    internal class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _repository;
        private readonly IMapper _mapper;
        public ProjectsService(IProjectsRepository repository, IMapper mapper)
        {
            _repository = repository;
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
                if (_repository.GetProjectByName(model.Name) != null)
                {
                    Log.Error($"Project {model.Name} already exists");
                    throw new CustomException($"Project {model.Name} already exists");
                }
                var addedProject = await _repository.AddProject(_mapper.Map<DAL.Entities.Project>(model));

                if (addedProject.Id > 0)
                {
                    Log.Information($"Project {addedProject.Name} added successfully");
                    return _mapper.Map<DTO.Project>(addedProject);
                }

                Log.Information($"Project {model.Name} could not be added");
                throw new CustomException($"Project could not be added");

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new DTO.Project();
        }

        public async Task<Project> DeleteProject(int id)
        {
            try
            {
                var project = await _repository.GetProjectById(id);
                if (project == null)
                {
                    Log.Information("Project not found");
                    throw new CustomException("Project not found");
                }

                project.IsDeleted = true;
                var deletedProject = await _repository.DeleteProject(project);
                if (deletedProject != null)
                {
                    Log.Information($"Project {deletedProject.Name} deleted successfully");
                    return _mapper.Map<DTO.Project>(deletedProject);

                }

                Log.Information($"Project {project.Name} could not be deleted");
                throw new CustomException("Project could not be deleted");

            }
            catch (CustomException ex)
            {
                throw;
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
                    Log.Information($"Project {project.Name} retrieved successfully");
                    return _mapper.Map<DTO.Project>(project);

                }

                Log.Information("Project could not be retrieved");
                throw new CustomException("Project not found");

            }
            catch (CustomException ex)
            {
                throw;
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
                    Log.Information("Projects retrieved successfully");
                    return _mapper.Map<List<DTO.Project>>(projects);
                }

                Log.Error("Projects could not be retrieved");
                throw new CustomException("Projects could not be retrieved");

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new List<Project>();
        }

        public async Task<Project> UpdateProject(int id, Project model)
        {
            try
            {
                var project = await _repository.GetProjectById(id);
                if (project == null)
                {
                    Log.Information($"Project not found");
                    throw new CustomException("Projects not found");

                }
                model.Id = id;
                var updated = await _repository.UpdateProject(_mapper.Map<DAL.Entities.Project>(model));
                if (updated != null)
                {
                    Log.Information($"Project {updated.Name} updated successfully");
                    return _mapper.Map<DTO.Project>(updated);
                }

                Log.Information($"Project {project.Name} could not be updated");
                throw new CustomException("Projects could not be updated");

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new Project();
        }

        public async Task<Project> GetProjectByName(string name)
        {
            try
            {
                var project = await _repository.GetProjectByName(name);
                if (project != null)
                {
                    Log.Information($"Project {name} retrieved successfully");
                    return _mapper.Map<DTO.Project>(project);

                }

                Log.Information($"Project {name} could not be retrieved");

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new DTO.Project();
        }
    }
}
