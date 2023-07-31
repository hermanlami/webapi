using AutoMapper;
using Serilog;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common;
using TaskManagementSystem.Common.CustomExceptions;
using TaskManagementSystem.Common.Enums;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class ProjectManagersService : IProjectManagersService
    {
        private readonly IProjectManagersRepository _repository;
        private readonly ITokensService _tokensService;
        private readonly IAuthenticationsRepository _authenticationsRepository;
        private readonly IMapper _mapper;
        public ProjectManagersService(IProjectManagersRepository repository, ITokensService tokensService, IAuthenticationsRepository authenticationsRepository,IMapper mapper)
        {
            _repository = repository;
            _tokensService = tokensService;
            _authenticationsRepository= authenticationsRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Krijon ne menaxher te ri projekti.
        /// </summary>
        /// <param name="model">Modeli qe sherben per te krijuar menaxherin e ri te projetit.</param>
        /// <returns>Menaxherin e krijuar.</returns>
        public async Task<ProjectManager> AddProjectManager(ProjectManager model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                if (await _authenticationsRepository.GetPersonByEmail(model.Email) != null)
                {
                    Log.Error($"Project manager with email {model.Email} already exists");
                    throw new DuplicateInputException($"Project manager with email {model.Email} already exists");
                }
                if (await _authenticationsRepository.GetPersonByUsername(model.Username) != null)
                {
                    Log.Error($"project Manager with username {model.Username} already exists");
                    throw new DuplicateInputException($"Project Manager with username {model.Username} already exists");
                }

                var dalPM = _mapper.Map<DAL.Entities.ProjectManager>(model);
                dalPM.PasswordHash = PasswordHashing.HashPasword(model.Password, out byte[] salt);
                dalPM.PasswordSalt = salt;
                dalPM.PersonType = PersonType.ProjectManager;
                var addedPM = await _repository.AddProjectManager(dalPM);

                if (addedPM.Id > 0)
                {
                    Mail.CredentialsNotification(addedPM.Email, model.Password);
                    Log.Information($"Project manager with username {addedPM.Username} added successfully");
                    return _mapper.Map<ProjectManager>(addedPM);
                }

                Log.Error($"Project manager with username {dalPM.Username} could not be added");
                throw new CustomException($"Project manager could not be added");

            });
        }
        /// <summary>
        /// Fshin nje menaxher projekti.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar menaxherin e projektit.</param>
        /// <returns>Menaxherin e fshire.</returns>
        public async Task<ProjectManager> DeleteProjectManager(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var pM = await _repository.GetProjectManagerById(id);
                if (pM == null)
                {
                    Log.Error("Project manager not found");
                    throw new NotFoundException($"Project manager not found");
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


            });
        }
        /// <summary>
        /// Merr nje menaxher projekti ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar menaxherin e projektit.</param>
        /// <returns>Menaxherin perkates.</returns>
        public async Task<ProjectManager> GetProjectManagerById(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var pM = await _repository.GetProjectManagerById(id);
                if (pM != null)
                {
                    Log.Information($"Project manager with username {pM.Username} retrieved successfully");
                    return _mapper.Map<ProjectManager>(pM);
                }

                Log.Error("Project manager could not be retrieved");
                throw new NotFoundException($"Project manager not found");


            });
        }
        /// <summary>
        /// Merr te gjithe menaxheret e projekteve.
        /// </summary>
        /// <returns>Listen e te gjithe menaxhereve.</returns>
        public async Task<List<ProjectManager>> GetProjectManagers()
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var pMs = await _repository.GetProjectManagers();
                if (pMs != null)
                {
                    Log.Information("Project managers retrieved successfully");
                    return _mapper.Map<List<ProjectManager>>(pMs);
                }

                Log.Error("Project managers could not be retrieved");
                throw new CustomException($"Project managers could not be retrieved");


            });
        }
        /// <summary>
        /// Perditeson menaxherin e projektit.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar menaxherin e projektit.</param>
        /// <param name="model">Modeli ne baze te te cilit do te perditesohen te dhenat.</param>
        /// <returns>Menaxherin e perditesuar.</returns>
        public async Task<ProjectManager> UpdateProjectManager(int id, ProjectManager model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                if (await _authenticationsRepository.GetPersonByEmail(model.Email, model.Id) != null)
                {
                    Log.Error($"Project manager with email {model.Email} already exists");
                    throw new DuplicateInputException($"Project manager with email {model.Email} already exists");
                }
                if (await _authenticationsRepository.GetPersonByUsername(model.Username, model.Id) != null)
                {
                    Log.Error($"project Manager with username {model.Username} already exists");
                    throw new DuplicateInputException($"Project Manager with username {model.Username} already exists");
                }

                var pM = await _repository.GetProjectManagerById(id);
                if (pM == null)
                {
                    Log.Error("Project manager not found");
                    throw new NotFoundException($"Project manager not found");
                }

                model.Id=pM.Id;

                var updated = await _repository.UpdateProjectManager(_mapper.Map(model, pM));
                if (updated != null)
                {
                    Log.Information($"Project manager with username {updated.Username} updated successfully");
                    return _mapper.Map<ProjectManager>(updated);
                }

                Log.Error($"Project manager with username {pM.Username} could not be updated");
                throw new CustomException($"Project manager could not be updated");
            });
        }
    }
}
