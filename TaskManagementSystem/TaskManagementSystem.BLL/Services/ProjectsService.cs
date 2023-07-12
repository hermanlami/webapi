.using AutoMapper;
using Serilog;
using System.Runtime.CompilerServices;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
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
                var dalProject = _mapper.Map<DAL.Entities.Project>(model);

                var addedProject = await _repository.AddProject(dalProject);

                if (addedProject.Id > 0)
                {
                    Log.Information("Project added successfully");
                    return _mapper.Map<DTO.Project>(addedProject);
                }
                else
                {
                    Log.Error("Project could not be added");

                }

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
                if (project != null)
                {
                    project.IsDeleted = true;
                    var deletedProject = await _repository.DeleteProject(project);
                    if (deletedProject != null)
                    {
                        Log.Information("Project deleted successfully");

                        return _mapper.Map<DTO.Project>(deletedProject);

                    }
                    else
                    {
                        Log.Error("Project could not be deleted");

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
                    Log.Information("Project retrieved successfully");

                    return _mapper.Map<DTO.Project>(project);

                }
                else
                {
                    Log.Error("Project could not be retrieved");

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
                    Log.Information("Projects retrieved successfully");

                    return _mapper.Map<List<DTO.Project>>(projects);
                }
                else
                {
                    Log.Error("Projects could not be retrieved");

                }
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
                if (project != null)
                {
                    model.Id = id;
                    var updated = await _repository.UpdateProject(_mapper.Map<DAL.Entities.Project>(model));
                    if (updated != null)
                    {
                        Log.Information("Project updated successfully");

                        return _mapper.Map<DTO.Project>(updated);
                    }
                    else
                    {
                            Log.Error("Project could not be updated");

                    }
                }
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
                    Log.Information("Project retrieved successfully");

                    return _mapper.Map<DTO.Project>(project);

                }
                else
                {
                    Log.Error("Project could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Project();
        }
    }
}
