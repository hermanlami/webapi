using Microsoft.Extensions.Logging;
using Serilog;
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
    internal class ProjectManagersService : IProjectManagersService
    {
        private readonly IProjectManagersRepository _repository;
        private readonly ILogger<ProjectManagersService> _logger;
        public ProjectManagersService(IProjectManagersRepository repository, ILogger<ProjectManagersService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<ProjectManager> AddProjectManager(ProjectManager model)
        {
            try
            {
                var dalPM = new DAL.Entities.ProjectManager()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PersonType = model.PersonType,
                    Birthday = model.Birthday,
                };
                var addedPM = await _repository.AddProjectManager(dalPM);
                model.Id = addedPM.Id;
                if (addedPM.Id > 0)
                {
                    _logger.LogInformation("Project manager added successfully");
                    return model;
                }
                else
                {
                    _logger.LogError("Project manager could not be added");
                }

            }
            catch (Exception ex)
            {

            }
            return new DTO.ProjectManager();
        }

        public async Task<ProjectManager> DeleteProjectManager(ProjectManager model)
        {
            try
            {
                var pM = await _repository.GetProjectManagerById(model.Id);
                if (pM != null)
                {
                    pM.IsDeleted = true;
                    var deletedPM = await _repository.DeleteProjectManager(pM);
                    if (deletedPM != null)
                    {
                        _logger.LogInformation("Project manager deleted successfully");

                        return new DTO.ProjectManager()
                        {
                            FirstName = deletedPM.FirstName,
                            LastName = deletedPM.LastName,
                            PersonType = deletedPM.PersonType,
                            Birthday = deletedPM.Birthday,
                        };

                    }
                    else
                    {
                        _logger.LogError("Project manager could not be deleted");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.ProjectManager();
        }

        public async Task<ProjectManager> GetProjectManagerById(int id)
        {
            try
            {
                var pM = await _repository.GetProjectManagerById(id);
                if (pM != null)
                {
                    _logger.LogInformation("Project manager retrieved successfully");

                    return new DTO.ProjectManager()
                    {
                        Id = pM.Id,
                        FirstName = pM.FirstName,
                        LastName = pM.LastName,
                        PersonType = pM.PersonType,
                        Birthday = pM.Birthday,
                    };
                }
                else
                {
                    _logger.LogError("Project manager could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.ProjectManager();
        }

        public async Task<ProjectManager> GetProjectManagerByEmail(string email)
        {
            try
            {
                var pM = await _repository.GetProjectManagerByEmail(email);
                if (pM != null)
                {
                    _logger.LogInformation("Project manager retrieved successfully");

                    return new DTO.ProjectManager()
                    {
                        Id = pM.Id,
                        FirstName = pM.FirstName,
                        LastName = pM.LastName,
                        Email = pM.Email,
                        UserName = pM.Username,
                        Password = pM.Password,
                        PersonType = pM.PersonType,
                        Birthday = pM.Birthday,
                    };
                }
                else
                {
                    _logger.LogError("Project manager could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.ProjectManager();
        }

        public async Task<List<ProjectManager>> GetProjectManagers()
        {
            try
            {
                var pMs = await _repository.GetProjectManagers();
                var dtoPMs = new List<ProjectManager>();
                if (pMs != null)
                {
                    _logger.LogInformation("Project managers retrieved successfully");

                    pMs.ForEach(x => dtoPMs.Add(new ProjectManager
                    {
                        Id = x.Id,
                        Birthday = x.Birthday,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        PersonType = x.PersonType
                    }));
                    return dtoPMs;
                }
                else
                {
                    _logger.LogError("Project managers could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new List<ProjectManager>();
        }

        public async Task<ProjectManager> UpdateProjectManager(ProjectManager model)
        {
            try
            {
                var pM = await _repository.GetProjectManagerById(model.Id);
                if (pM != null)
                {
                    pM.FirstName = model.FirstName;
                    pM.LastName = model.LastName;
                    pM.PersonType = model.PersonType;
                    pM.Birthday = model.Birthday;
                    var updated = await _repository.UpdateProjectManager(pM);
                    if (updated != null)
                    {
                        _logger.LogInformation("Project manager updated successfully");

                        return new ProjectManager()
                        {
                            FirstName = updated.FirstName,
                            LastName = updated.LastName,
                            PersonType = updated.PersonType,
                            Birthday = updated.Birthday,
                        };
                    }
                    else
                    {
                        _logger.LogError("Project manager could not be updated");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new ProjectManager();
        }
    }
}
