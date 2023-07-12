using AutoMapper;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common;
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
                var dalDeveloper = _mapper.Map<DAL.Entities.Developer>(model);
                byte[] salt;
                dalDeveloper.PasswordHash = PasswordHashing.HashPasword(model.Password, out salt);
                dalDeveloper.PasswordSalt = salt;
                var addedDeveloper = await _repository.AddDeveloper(dalDeveloper);

                if (addedDeveloper.Id > 0)
                {
                    Log.Information("Developer added successfully");
                    return _mapper.Map<DTO.Developer>(addedDeveloper);
                }
                else
                {
                    Log.Error("Developer could not be added");

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

        public async Task<Developer> DeleteDeveloper(int id)
        {
            try
            {
                var developer = await _repository.GetDeveloperById(id);
                if (developer != null)
                {
                    developer.IsDeleted = true;

                    var deletedDeveloper = await _repository.DeleteDeveloper(developer);
                    if (deletedDeveloper != null)
                    {
                        Log.Information("Developer deleted successfully");

                        return _mapper.Map<DTO.Developer>(deletedDeveloper);

                    }
                    else
                    {
                        Log.Error("Developer could not be deleted");

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
                    Log.Information("Developer retrieved successfully");

                    return _mapper.Map<DTO.Developer>(developer);
                }
                else
                {
                    Log.Error("Developer could not be retrieved");

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
                    Log.Information("Developer retrieved successfully");

                    return _mapper.Map<DTO.Developer>(developer);
                }
                else
                {
                    Log.Error("Developer could not be retrieved");

                }
            }
            catch (Exception ex)
            {

            }
            return new DTO.Developer();
        }

        public async Task<Developer> GetDeveloperByUsername(string name)
        { 
            try
            {
                var developer = await _repository.GetDeveloperByUsername(name);
                if (developer != null)
                {
                    Log.Information("Developer retrieved successfully");

                    return _mapper.Map<DTO.Developer>(developer);
                }
                else
                {
                    Log.Error("Developer could not be retrieved");

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
                    Log.Information("Developers retrieved successfully");

                    return _mapper.Map<List<DTO.Developer>>(developers);
                }
                else
                {
                    Log.Error("Developers could not be retrieved");

                }
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
                if (developer != null)
                {
                    model.Id = id;
                    var updated = await _repository.UpdateDeveloper(_mapper.Map<DAL.Entities.Developer>(model));
                    if (updated != null)
                    {
                        Log.Information("Developer updated successfully");

                        return _mapper.Map<DTO.Developer>(updated);
                    }
                    else
                    {
                        Log.Error("Developer could not be updated");

                    }
                }
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
            catch (Exception ex)
            {

            }
            return new AuthenticationResponse();
        }
    }
}
