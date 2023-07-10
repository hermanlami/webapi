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
using TaskManagementSystem.Common;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class DevelopersService : IDevelopersService
    {
        private readonly IDevelopersRepository _repository;
        private readonly ILogger<DevelopersService> _logger;
        private readonly IMapper _mapper;
        public DevelopersService(IDevelopersRepository repository, ILogger<DevelopersService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Developer> AddDeveloper(Developer model)
        {
            try
            {
                var dalDeveloper = _mapper.Map<DAL.Entities.Developer>(model);

                var addedDeveloper = await _repository.AddDeveloper(dalDeveloper);

                if (addedDeveloper.Id > 0)
                {
                    _logger.LogInformation("Developer added successfully");
                    return _mapper.Map<DTO.Developer>(addedDeveloper);
                }
                else
                {
                    _logger.LogError("Developer could not be added");

                }

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new DTO.Developer();
        }

        public async Task<Developer> DeleteDeveloper(Developer model)
        {
            try
            {
                var developer = await _repository.GetDeveloperById(model.Id);
                if (developer != null)
                {
                    developer.IsDeleted = true;

                    var deletedDeveloper = await _repository.DeleteDeveloper(developer);
                    if (deletedDeveloper != null)
                    {
                        _logger.LogInformation("Developer deleted successfully");

                        return _mapper.Map<DTO.Developer>(deletedDeveloper);

                    }
                    else
                    {
                        _logger.LogError("Developer could not be deleted");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Developer();
        }

        public async Task<Developer> GetDeveloperById(int id)
        {
            try
            {
                var developer = await _repository.GetDeveloperById(id);
                if (developer != null)
                {
                    _logger.LogInformation("Developer retrieved successfully");

                    return _mapper.Map<DTO.Developer>(developer);
                }
                else
                {
                    _logger.LogError("Developer could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Developer();
        }

        public async Task<Developer> GetDeveloperByEmail(string email)
        {
            try
            {
                var developer = await _repository.GetDeveloperByEmail(email);
                if (developer != null) 
                {
                    _logger.LogInformation("Developer retrieved successfully"); 

                    return _mapper.Map<DTO.Developer>(developer);
                }
                else
                {
                    _logger.LogError("Developer could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Developer();
        }

        public async Task<List<Developer>> GetDevelopers()
        {
            try
            {
                var developers = await _repository.GetDevelopers();
                if (developers != null)
                {
                    _logger.LogInformation("Developers retrieved successfully");

                    return _mapper.Map<List<DTO.Developer>>(developers);
                }
                else
                {
                    _logger.LogError("Developers could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new List<Developer>();
        }

        public async Task<Developer> UpdateDeveloper(Developer model)
        {
            try
            {
                var developer = await _repository.GetDeveloperById(model.Id);
                if (developer != null)
                {
                    developer.FirstName = model.FirstName;
                    developer.LastName = model.LastName;
                    developer.ManagerId = model.ManagerId;
                    developer.PersonType = model.PersonType;
                    developer.Birthday = model.Birthday;
                    var updated = await _repository.UpdateDeveloper(developer);
                    if (updated != null)
                    {
                        _logger.LogInformation("Developer updated successfully");

                        return _mapper.Map<DTO.Developer>(updated);
                    }
                    else
                    {
                        _logger.LogError("Developer could not be updated");

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new Developer();
        }
    }
}
