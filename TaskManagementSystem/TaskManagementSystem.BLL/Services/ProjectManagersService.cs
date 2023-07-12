using AutoMapper;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
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
                var dalPM = _mapper.Map<DAL.Entities.ProjectManager>(model);
                byte[] salt;
                dalPM.PasswordHash = PasswordHashing.HashPasword(model.Password, out salt);
                dalPM.PasswordSalt = salt;
                var addedPM = await _repository.AddProjectManager(dalPM);

                if (addedPM.Id > 0)
                {
                    Log.Information("Project manager added successfully");
                    return _mapper.Map<ProjectManager>(addedPM);
                }
                else
                {
                    Log.Error("Project manager could not be added");
                }

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
                if (pM != null)
                {
                    pM.IsDeleted = true;
                    var deletedPM = await _repository.DeleteProjectManager(pM);
                    if (deletedPM != null)
                    {
                        Log.Information("Project manager deleted successfully");

                        return _mapper.Map<ProjectManager>(deletedPM);

                    }
                    else
                    {
                        Log.Error("Project manager could not be deleted");

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
                    Log.Information("Project manager retrieved successfully");

                    return _mapper.Map<ProjectManager>(pM);
                }
                else
                {
                    Log.Error("Project manager could not be retrieved");

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
                    Log.Information("Project manager retrieved successfully");

                    return _mapper.Map<ProjectManager>(pM);
                }
                else
                {
                    Log.Error("Project manager could not be retrieved");

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
                    Log.Information("Project managers retrieved successfully");

                    return _mapper.Map<List<ProjectManager>>(pMs);
                }
                else
                {
                    Log.Error("Project managers could not be retrieved");

                }
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
                if (pM != null)
                {
                    model.Id = id;
                    var updated = await _repository.UpdateProjectManager(_mapper.Map<DAL.Entities.ProjectManager>(model));
                    if (updated != null)
                    {
                        Log.Information("Project manager updated successfully");

                        return _mapper.Map<ProjectManager>(updated);
                    }
                    else
                    {
                        Log.Error("Project manager could not be updated");

                    }
                }
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
            catch (Exception ex)
            {

            }
            return new AuthenticationResponse();
        }
    }
}
