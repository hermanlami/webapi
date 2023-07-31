using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Resources;
using Serilog.Events;
using System.Globalization;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common.CustomExceptions;
using TaskManagementSystem.DAL.Interfaces;
using TaskManagementSystem.BLL.Resources;

namespace TaskManagementSystem.BLL.Services
{
    public class AuthenticationsService : IAuthenticationsService
    {
        private readonly IAuthenticationsRepository _peoplesRepository;
        private readonly ITokensService _tokensService;
        private readonly ILogger<AuthenticationsService> _logger;
        private readonly IStringLocalizer<LogMessages> _localizer;
        private readonly IMapper _mapper;
        public AuthenticationsService(IAuthenticationsRepository peoplesRepository, ITokensService tokensService, ILogger<AuthenticationsService> logger, IStringLocalizer<LogMessages> localizer, IMapper mapper)
        {
            _peoplesRepository = peoplesRepository;
            _tokensService = tokensService;
            _logger = logger;
            _localizer = localizer;
            _mapper = mapper;
        }
        /// <summary>
        /// Ben autentifikim e perdoruesit ne baze te kredencialeve te tij.
        /// </summary>
        /// <param name="request">Modeli e permban kredencialet e perdoruesit.</param>
        /// <returns>Kredencialet e perdoruesit, token per autentifikim dhe autorizim si dhe refresh token per te shmangur logimin
        /// e perseritur kur perdoruesi eshte ende aktiv ne momentin e skadimit te token.</returns>
        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var person = await _peoplesRepository.GetPersonByEmail(request.Email);
                if (person == null)
                {
                    var s = GetLogValue("NotFound");
                    _logger.LogError($"{GetLogValue("NotFound")}");
                    throw new NotFoundException($"Person with email {request.Email} not found");
                }
                if (person != null && PasswordHashing.VerifyPassword(request.Password, person.PasswordHash, person.PasswordSalt))
                {
                    var accessToken = _tokensService.CreateToken(person);
                    return new AuthenticationResponse
                    {
                        Username = person.Username,
                        Email = person.Email,
                        Token = accessToken.AccessToken,
                        RefreshToken = accessToken.RefreshToken,
                    };
                }
                _logger.LogError($"{GetLogValue("NotAuthenticated")}");
                throw new FailedToAuthencitcateException($"Wrong password! Person with email {request.Email} could not be authenticated");

            });
        }
        /// <summary>
        /// Ndryshon password-in e perdoruesit kur ai mund te jape sakte password-in e vjeter.
        /// </summary>
        /// <param name="id">Id e perdoruesit te loguar qe deshiron te ndryshoje passwordin.</param>
        /// <param name="model">Modeli qe permban passwordin e ri si dhe password-in e vjeter si mase kontrolluese.</param>
        /// <returns>Perdoruesin me password-in e perditesuar.</returns>
        public async Task<Person> ChangePassword(int id, UpdatePasswordRequest model)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var person = await _peoplesRepository.GetPersonById(id);
                if (person == null)
                {
                    _logger.LogInformation($"{GetLogValue("NotFound")}");
                    throw new NotFoundException($"User not found");
                }
                if (!PasswordHashing.VerifyPassword(model.OldPassword, person.PasswordHash, person.PasswordSalt))
                {
                    _logger.LogInformation($"{GetLogValue("IncorrectPassword")}");
                    throw new CustomException($"Old password value is incorrect");
                }

                person.PasswordHash = PasswordHashing.HashPasword(model.NewPassword, out byte[] salt);
                person.PasswordSalt = salt;

                var updated = await _peoplesRepository.ChangePassword(person);
                if (updated != null)
                {
                    _logger.LogInformation($"{GetLogValue("UpdateSuccessful")}");
                    return _mapper.Map<Person>(updated);
                }

                _logger.LogInformation($"{GetLogValue("UpdateFailed")}");

                throw new CustomException($"Project manager could not be updated");
            });
        }
        /// <summary>
        /// Merr nje person ne baze te adreses se tij email.
        /// </summary>
        /// <param name="email">Email qe sherben per te identifikuar personin.</param>
        /// <returns>Personin perkates.</returns>
        public async Task<Person> GetPersonByEmail(string email,int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var person = await _peoplesRepository.GetPersonByEmail(email,id);
                if (person != null)
                {
                    _logger.LogInformation($"{GetLogValue("RetrieveSuccessful")}");
                    return _mapper.Map<DTO.Person>(person);
                }
                _logger.LogInformation($"{GetLogValue("NotFound")}");
                throw new NotFoundException($"Person not found");

            });
        }
        /// <summary>
        /// Merr nje person ne baze te username-it te tij.
        /// </summary>
        /// <param name="username">Username qe sherben per te identifikuar personin.</param>
        /// <returns>Personin perkates.</returns>
        public async Task<Person> GetPersonByUsername(string username, int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var person = await _peoplesRepository.GetPersonByUsername(username,id);
                if (person != null)
                {
                    _logger.LogInformation($"{GetLogValue("RetrieveSuccessful")}");
                    return _mapper.Map<DTO.Person>(person);
                }

                _logger.LogInformation($"{GetLogValue("NotFound")}");
                throw new NotFoundException($"Person not found");

            });
        }
        /// <summary>
        /// Merr nje person ne baze te Id se tij.
        /// </summary>
        /// <param name="id">Id qe sherben per te identifikuar personin.</param>
        /// <returns>Personin perkates.</returns>
        public async Task<Person> GetPersonById(int id)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var person = await _peoplesRepository.GetPersonById(id);
                if (person != null)
                {
                    _logger.LogInformation($"{GetLogValue("RetrieveSuccessful")}");
                    return _mapper.Map<DTO.Person>(person);
                }

                _logger.LogInformation($"{GetLogValue("NotFound")}");
                throw new NotFoundException($"Person not found");

            });
        }
        /// <summary>
        /// Merr vleren e mesazhit sipas celesit identifikues ne file-t per lokalizimin e aplikacionit.
        /// </summary>
        /// <param name="key">Celesi qe identifikin mesazhin ne file-t sipas gjuhes se perzgjedhur.</param>
        /// <returns>Vleren perkatese.</returns>
        private string GetLogValue(string key)
        {
            var cultureInfo = CultureInfo.CurrentCulture;
            var resourceManager = new ResourceManager(typeof(LogMessages));
            return resourceManager.GetString(key, cultureInfo); 
        }
    }
}
