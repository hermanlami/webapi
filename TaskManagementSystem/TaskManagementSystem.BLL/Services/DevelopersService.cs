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
        public DevelopersService(IDevelopersRepository repository, ILogger<DevelopersService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<Developer> AddDeveloper(Developer model)
        {
            try
            {
                var dalDeveloper = new DAL.Entities.Developer()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PersonType = model.PersonType,
                    Birthday = model.Birthday,
                    ManagerId = model.ManagerId,
                    Username = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                };
                var addedDeveloper = await _repository.AddDeveloper(dalDeveloper);
                if (model.ManagerId == 10)
                {
                    throw new CustomException("Manager is not available!.");
                }
                model.Id = addedDeveloper.Id;
                if (addedDeveloper.Id > 0)
                {
                    _logger.LogInformation("Developer added successfully");
                    return model;
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

                        return new DTO.Developer()
                        {
                            FirstName = deletedDeveloper.FirstName,
                            LastName = deletedDeveloper.LastName,
                            PersonType = deletedDeveloper.PersonType,
                            Birthday = deletedDeveloper.Birthday,
                            ManagerId = deletedDeveloper.ManagerId,
                        };

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

                    return new DTO.Developer()
                    {
                        Id = developer.Id,
                        FirstName = developer.FirstName,
                        LastName = developer.LastName,
                        PersonType = developer.PersonType,
                        Birthday = developer.Birthday,
                        ManagerId = developer.ManagerId,
                    };
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

                    return new DTO.Developer()
                    {
                        Id = developer.Id,
                        FirstName = developer.FirstName,
                        LastName = developer.LastName,
                        Email = developer.Email,
                        UserName= developer.Username,
                        Password= developer.Password,
                        PersonType = developer.PersonType,
                        Birthday = developer.Birthday,
                        ManagerId = developer.ManagerId,
                    };
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
                var dtoDevelopers = new List<Developer>();
                if (developers != null)
                {
                    _logger.LogInformation("Developers retrieved successfully");

                    developers.ForEach(x => dtoDevelopers.Add(new Developer
                    {
                        Id = x.Id,
                        Birthday = x.Birthday,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        ManagerId = x.ManagerId,
                        PersonType = x.PersonType
                    }));
                    return dtoDevelopers;
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

                        return new Developer()
                        {
                            FirstName = updated.FirstName,
                            LastName = updated.LastName,
                            ManagerId = updated.ManagerId,
                            PersonType = updated.PersonType,
                            Birthday = updated.Birthday,
                        };
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
