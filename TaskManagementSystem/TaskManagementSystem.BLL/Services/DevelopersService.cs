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
        public async Task<Developer> AddDeveloper(Developer model)
        {
            try
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
                    Log.Information($"Developer with username {addedDeveloper.Username} added successfully");
                    return _mapper.Map<DTO.Developer>(addedDeveloper);
                }

                Log.Information($"Developer with username {dalDeveloper.Username} could not be added");
                throw new CustomException($"Developer could not be added");

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

        public async Task<Developer> DeleteDeveloper(int id)
        {
            try
            {
                var developer = await _repository.GetDeveloperById(id);
                if (developer == null)
                {
                    Log.Information("Developer not found");
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

        public async Task<Developer> GetDeveloperById(int id)
        {
            try
            {
                var developer = await _repository.GetDeveloperById(id);
                if (developer != null)
                {
                    Log.Information($"Developer with username {developer.Username} retrieved successfully");
                    return _mapper.Map<DTO.Developer>(developer);
                }
                Log.Information("Developer could not be retrieved");
                throw new CustomException("Developer not found");
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

        public async Task<Developer> GetDeveloperByEmail(string email)
        {
            try
            {
                var developer = await _repository.GetDeveloperByEmail(email);
                if (developer != null)
                {
                    Log.Information($"Developer with username {developer.Username} retrieved successfully");
                    return _mapper.Map<DTO.Developer>(developer);
                }
                Log.Information("Developer could not be retrieved");
            }
            catch (CustomException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return new DTO.Developer();
        }

        public async Task<Developer> GetDeveloperByUsername(string username)
        {
            try
            {
                var developer = await _repository.GetDeveloperByUsername(username);
                if (developer != null)
                {
                    Log.Information($"Developer with username {developer.Username} retrieved successfully");
                    return _mapper.Map<DTO.Developer>(developer);
                }

                Log.Error("Developer could not be retrieved");
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
                    Log.Information("Developers retrieved successfully");
                    return _mapper.Map<List<DTO.Developer>>(developers);
                }

                Log.Information("Developers could not be retrieved");
                throw new CustomException($"Developers could not be retrieved");

            }
            catch (Exception ex)
            {

            }
            return new List<Developer>();
        }

        public async Task<Developer> UpdateDeveloper(int id, Developer model)
        {
            try
            {
                var developer = await _repository.GetDeveloperById(id);
                if (developer == null)
                {
                    Log.Information("Developer not found");
                    throw new CustomException("Developer could not be found");
                }

                model.Id = id;
                var updated = await _repository.UpdateDeveloper(_mapper.Map<DAL.Entities.Developer>(model));
                if (updated != null)
                {
                    Log.Information($"Developer with username {updated.Username} updated successfully");
                    return _mapper.Map<DTO.Developer>(updated);
                }

                Log.Information($"Developer with username {developer.Username} could not be updated");
                throw new CustomException("Developer could not be updated");

            }
            catch (Exception ex)
            {

            }
            return new Developer();
        }
        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            try
            {
                var developer = await _repository.GetDeveloperByEmail(request.Email);
                if (developer == null)
                {
                    throw new CustomException($"Developer with email {request.Email} not found");
                }
                if (developer != null && PasswordHashing.VerifyPassword(request.Password, developer.PasswordHash, developer.PasswordSalt))
                {
                    var accessToken = _tokensService.CreateToken(developer);
                    return new AuthenticationResponse
                    {
                        Username = developer.Username,
                        Email = developer.Email,
                        Token = accessToken.AccessToken,
                    };
                }
            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new AuthenticationResponse();
        }
    }
}
