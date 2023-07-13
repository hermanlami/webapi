using AutoMapper;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common;
using TaskManagementSystem.Common.Enums;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class ProjectManagersService : IProjectManagersService
    {
        private readonly IProjectManagersRepository _repository;
        private readonly ITokensService _tokensService;
        private readonly IMapper _mapper;
        public ProjectManagersService(IProjectManagersRepository repository, ITokensService tokensService, IMapper mapper)
        {
            _repository = repository;
            _tokensService = tokensService;
            _mapper = mapper;
        }
        public async Task<ProjectManager> AddProjectManager(ProjectManager model)
        {
            try
            {
                if (await _repository.GetProjectManagerByEmail(model.Email) != null)
                {
                    Log.Error($"Project manager with email {model.Email} already exists");
                    throw new CustomException($"Project manager with email {model.Email} already exists");
                }
                var dalPM = _mapper.Map<DAL.Entities.ProjectManager>(model);
                dalPM.PasswordHash = PasswordHashing.HashPasword(model.Password, out byte[] salt);
                dalPM.PasswordSalt = salt;
                dalPM.PersonType = PersonType.ProjectManager;
                var addedPM = await _repository.AddProjectManager(dalPM);

                if (addedPM.Id > 0)
                {
                    Log.Information($"Project manager with username {addedPM.Username} added successfully");
                    return _mapper.Map<ProjectManager>(addedPM);
                }

                Log.Information($"Project manager with username {dalPM.Username} could not be added");
                throw new CustomException($"Project manager could not be added");

            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new DTO.ProjectManager();
        }

        public async Task<ProjectManager> DeleteProjectManager(int id)
        {
            try
            {
                var pM = await _repository.GetProjectManagerById(id);
                if (pM == null)
                {
                    Log.Information("Project manager not found");
                    throw new CustomException($"Project manager not found");
                }

                pM.IsDeleted = true;
                var deletedPM = await _repository.DeleteProjectManager(pM);
                if (deletedPM != null)
                {
                    Log.Information($"Project manager with username {deletedPM.Username} deleted successfully");
                    return _mapper.Map<ProjectManager>(deletedPM);

                }

                Log.Error($"Project manager with username {pM.Username} could not be deleted");
                throw new CustomException($"Project manager {pM.Username} could not be deleted");


            }
            catch (CustomException ex)
            {
                throw;
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
                    Log.Information($"Project manager with username {pM.Username} retrieved successfully");
                    return _mapper.Map<ProjectManager>(pM);
                }

                Log.Information("Project manager could not be retrieved");
                throw new CustomException($"Project manager not found");


            }
            catch (CustomException ex)
            {
                throw;
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
                    Log.Information($"Project manager with username {pM.Username} retrieved successfully");
                    return _mapper.Map<ProjectManager>(pM);
                }

                Log.Information("Project manager could not be retrieved");

            }
            catch (CustomException ex)
            {
                throw;
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
                    Log.Information("Project managers retrieved successfully");
                    return _mapper.Map<List<ProjectManager>>(pMs);
                }

                Log.Information("Project managers could not be retrieved");
                throw new CustomException($"Project managers could not be retrieved");


            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new List<ProjectManager>();
        }

        public async Task<ProjectManager> UpdateProjectManager(int id, ProjectManager model)
        {
            try
            {
                var pM = await _repository.GetProjectManagerById(id);
                if (pM == null)
                {
                    Log.Information("Project manager not found");
                    throw new CustomException($"Project manager not found");
                }
                model.Id = id;
                var updated = await _repository.UpdateProjectManager(_mapper.Map<DAL.Entities.ProjectManager>(model));
                if (updated != null)
                {
                    Log.Information($"Project manager with username {updated.Username} updated successfully");
                    return _mapper.Map<ProjectManager>(updated);
                }

                Log.Information($"Project manager with username {pM.Username} could not be updated");
                throw new CustomException($"Project manager could not be updated");
            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return new ProjectManager();
        }
        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            try
            {
                var pM = await _repository.GetProjectManagerByEmail(request.Email);
                if(pM == null)
                {
                    throw new CustomException($"Project manager with email {request.Email} not found");
                }
                if (pM != null && PasswordHashing.VerifyPassword(request.Password, pM.PasswordHash, pM.PasswordSalt))
                {
                    var accessToken = _tokensService.CreateToken(pM);
                    return new AuthenticationResponse
                    {
                        Username = pM.Username,
                        Email = pM.Email,
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
