using AutoMapper;
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
        private readonly IMapper _mapper;
        public ProjectManagersService(IProjectManagersRepository repository, ILogger<ProjectManagersService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ProjectManager> AddProjectManager(ProjectManager model)
        {
            try
            {
                var dalPM = _mapper.Map<DAL.Entities.ProjectManager>(model);
                byte[] salt;
                dalPM.PasswordHash = PasswordHashing.HashPasword(model.Password, out salt);
                dalPM.PasswordSalt = salt;
                var addedPM = await _repository.AddProjectManager(dalPM);
                
                if (addedPM.Id > 0)
                {
                    _logger.LogInformation("Project manager added successfully");
                    return _mapper.Map<ProjectManager>(addedPM);
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

                        return _mapper.Map<ProjectManager>(deletedPM);

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

                    return _mapper.Map<ProjectManager>(pM);
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

                    return _mapper.Map<ProjectManager>(pM);
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
                if (pMs != null)
                {
                    _logger.LogInformation("Project managers retrieved successfully");

                    return _mapper.Map<List<ProjectManager>>(pMs);
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
                // me pak rreshta
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

                        return _mapper.Map<ProjectManager>(updated);
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
