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
        /// <summary>
        /// Krijon nje project te ri.
        /// </summary>
        /// <param name="model"> Modeli qe duhet te krijohet.</param>
        /// <returns>Projektin e krijuar.</returns>
        public async Task<Project> AddProject(Project model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                if (await _repository.GetProjectByName(model.Name) != null)
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

                Log.Error($"Project {model.Name} could not be added");
                throw new CustomException($"Project could not be added");
            });
        }
        /// <summary>
        /// Fshin nje projekt.
        /// </summary>
        /// <param name="id">Id qe identifikon projektin qe duhet fshire.</param>
        /// <returns>Projektin e fshire.</returns>
        public async Task<Project> DeleteProject(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var project = await _repository.GetProjectById(id);
                if (project == null)
                {
                    Log.Error("Project not found");
                    throw new CustomException("Project not found");
                }

                project.IsDeleted = true;
                var deletedProject = await _repository.DeleteProject(project);
                if (deletedProject != null)
                {
                    Log.Information($"Project {deletedProject.Name} deleted successfully");
                    return _mapper.Map<DTO.Project>(deletedProject);

                }

                Log.Error($"Project {project.Name} could not be deleted");
                throw new CustomException("Project could not be deleted");

            });
        }
        /// <summary>
        /// Merr nje projekt ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe identifikon projektin qe duhet marre.</param>
        /// <returns>Projektin perkates.</returns>
        public async Task<Project> GetProjectById(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var project = await _repository.GetProjectById(id);
                if (project != null)
                {
                    Log.Information($"Project {project.Name} retrieved successfully");
                    return _mapper.Map<DTO.Project>(project);

                }

                Log.Error("Project could not be retrieved");
                throw new CustomException("Project not found");

            });
        }
        /// <summary>
        /// Merr te gjitha projektet.
        /// </summary>
        /// <returns>Listen e te gjitha projekteve.</returns>
        public async Task<List<Project>> GetProjects()
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var projects = await _repository.GetProjects();
                if (projects != null)
                {
                    Log.Information("Projects retrieved successfully");
                    return _mapper.Map<List<DTO.Project>>(projects);
                }

                Log.Error("Projects could not be retrieved");
                throw new CustomException("Projects could not be retrieved");

            });
        }
        /// <summary>
        /// Perditeson nje projekt.
        /// </summary>
        /// <param name="id">Id e projektit qe duhet perditesuar.</param>
        /// <param name="model">Modeli qe sherben per te perditesuar vlerat e projektit te kapur nepermjet Id.</param>
        /// <returns>Projektin e perditesuar.</returns>
        public async Task<Project> UpdateProject(int id, Project model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var project = await _repository.GetProjectById(id);
                if (project == null)
                {
                    Log.Error($"Project not found");
                    throw new CustomException("Projects not found");

                }

                model.Id = id;
                
                var updated = await _repository.UpdateProject(_mapper.Map(model, project));
                if (updated != null)
                {
                    Log.Information($"Project {updated.Name} updated successfully");
                    return _mapper.Map<DTO.Project>(updated);
                }

                Log.Error($"Project {project.Name} could not be updated");
                throw new CustomException("Projects could not be updated");

            });
        }
        /// <summary>
        /// Merr nje projekt bazuar ne emrin e tij.
        /// </summary>
        /// <param name="name">Emri qe sherben per te identifikuar projektin.</param>
        /// <returns>Projektin perkates.</returns>
        public async Task<Project> GetProjectByName(string name)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var project = await _repository.GetProjectByName(name);
                if (project != null)
                {
                    Log.Information($"Project {name} retrieved successfully");
                    return _mapper.Map<DTO.Project>(project);

                }
                Log.Information($"Project {name} could not be retrieved");
                throw new CustomException($"Project {name} could not be retrieved");

            });
        }
    }
}
