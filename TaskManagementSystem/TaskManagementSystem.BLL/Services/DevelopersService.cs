using AutoMapper;
using Microsoft.Identity.Client;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common;
using TaskManagementSystem.Common.Enums;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class DevelopersService : IDevelopersService
    {
        private readonly IDevelopersRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITokensService _tokensService;
        public DevelopersService(IDevelopersRepository repository, IMapper mapper, ITokensService tokensService)
        {
            _repository = repository;
            _mapper = mapper;
            _tokensService = tokensService;
        }
        /// <summary>
        /// Krijon nje developer te ri.
        /// </summary>
        /// <param name="model">Modeli qe sherben per te krijuar developer-in e ri.</param>
        /// <returns>Developerin e krijuar.</returns>
        public async Task<Developer> AddDeveloper(Developer model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                if (await _repository.GetDeveloperByUsername(model.Username) != null)
                {
                    Log.Error($"Developer with username {model.Username} already exists");
                    throw new CustomException($"Developer with username {model.Username} already exists");
                }
                if (await _repository.GetDeveloperByEmail(model.Email) != null)
                {
                    Log.Error($"Developer with email {model.Email} already exists");
                    throw new CustomException($"Developer with email {model.Email} already exists");
                }

                var dalDeveloper = _mapper.Map<DAL.Entities.Developer>(model);

                dalDeveloper.PasswordHash = PasswordHashing.HashPasword(model.Password, out byte[] salt);

                dalDeveloper.PasswordSalt = salt;

                dalDeveloper.PersonType = PersonType.Developer;

                var addedDeveloper = await _repository.AddDeveloper(dalDeveloper);
                if (addedDeveloper.Id > 0)
                {
                    Mail.CredentialsNotification(addedDeveloper.Email, model.Password);
                    Log.Information($"Developer with username {addedDeveloper.Username} added successfully");
                    return _mapper.Map<DTO.Developer>(addedDeveloper);
                }

                Log.Error($"Developer with username {dalDeveloper.Username} could not be added");
                throw new CustomException($"Developer could not be added");

            });
        }
        /// <summary>
        /// Fshin nje developer.
        /// </summary>
        /// <param name="id">Id qe sherben per te idetifikuar developer-in qe duhet fshire.</param>
        /// <returns>Developer-in e fshire.</returns>
        public async Task<Developer> DeleteDeveloper(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var developer = await _repository.GetDeveloperById(id);
                if (developer == null)
                {
                    Log.Error("Developer not found");
                    throw new CustomException("Developer not found");
                }

                developer.IsDeleted = true;
               
                var deletedDeveloper = await _repository.DeleteDeveloper(developer);
                if (deletedDeveloper != null) 
                {
                    Log.Information($"Developer with username {deletedDeveloper.Username} deleted successfully");
                    return _mapper.Map<DTO.Developer>(deletedDeveloper);

                }

                Log.Error($"Developer with username {developer.Username} could not be deleted");
                throw new CustomException("Developer could not be deleted");

            });
        }
        /// <summary>
        /// Kap nje develoepr ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe sherben per te idetifikuar developer-in qe duhet fshire.</param>
        /// <returns>Developer-in perkates.</returns>
        public async Task<Developer> GetDeveloperById(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var developer = await _repository.GetDeveloperById(id);
                if (developer != null)
                {
                    Log.Information($"Developer with username {developer.Username} retrieved successfully");
                    return _mapper.Map<DTO.Developer>(developer);
                }
                Log.Error("Developer could not be retrieved");
                throw new CustomException("Developer not found");
            });
        }
        /// <summary>
        /// Merr te gjithe developers.
        /// </summary>
        /// <returns>Listen e te gjithe developers.</returns>
        public async Task<List<Developer>> GetDevelopers()
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var developers = await _repository.GetDevelopers();
                if (developers != null)
                {
                    Log.Information("Developers retrieved successfully");
                    return _mapper.Map<List<DTO.Developer>>(developers);
                }

                Log.Error("Developers could not be retrieved");
                throw new CustomException($"Developers could not be retrieved");

            });
        }
        /// <summary>
        /// Perditeson te dhenat e nje developer.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar developer.</param>
        /// <param name="model">Modeli ne baze te te cilit behet perditesimi i the dhenave.</param>
        /// <returns>Developer-in e perditesuar.</returns>
        public async Task<Developer> UpdateDeveloper(int id, Developer model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var developer = await _repository.GetDeveloperById(id);
                if (developer == null)
                {
                    Log.Error("Developer not found");
                    throw new CustomException("Developer could not be found");
                }

                model.Id = id;
                var updated = await _repository.UpdateDeveloper(_mapper.Map(model, developer));
                if (updated != null)
                {
                    Log.Information($"Developer with username {updated.Username} updated successfully");
                    return _mapper.Map<DTO.Developer>(updated);
                }

                Log.Error($"Developer with username {developer.Username} could not be updated");
                throw new CustomException("Developer could not be updated");

            });
        }
    }
}
