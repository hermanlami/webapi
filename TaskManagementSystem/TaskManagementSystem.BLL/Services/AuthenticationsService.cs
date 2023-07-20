using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common;
using TaskManagementSystem.DAL.Interfaces;
using TaskManagementSystem.DAL.Repositories;

namespace TaskManagementSystem.BLL.Services
{
    internal class AuthenticationsService : IAuthenticationsService
    {
        private readonly IAuthenticationsRepository _peoplesRepository;
        private readonly ITokensService _tokensService;
        private readonly IMapper _mapper;
        public AuthenticationsService(IAuthenticationsRepository peoplesRepository, ITokensService tokensService, IMapper mapper)
        {
            _peoplesRepository = peoplesRepository;
            _tokensService = tokensService;
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
                    throw new CustomException($"Person with email {request.Email} not found");
                }
                if (person != null && PasswordHashing.VerifyPassword(request.Password, person.PasswordHash, person.PasswordSalt))
                {
                    var accessToken = _tokensService.CreateToken(person);
                    return new AuthenticationResponse
                    {
                        Username = person.Username,
                        Email = person.Email,
                        Token = accessToken.AccessToken,
                        RefreshToken=accessToken.RefreshToken,
                    };
                }
                throw new CustomException($"Person with email {request.Email} could not be authenticated");

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
                    Log.Information("User not found");
                    throw new CustomException($"User not found");
                }
                if (!PasswordHashing.VerifyPassword(model.OldPassword, person.PasswordHash, person.PasswordSalt))
                {
                    Log.Error("Old password value is incorrect");
                    throw new CustomException($"Old password value is incorrect");
                }

                person.PasswordHash = PasswordHashing.HashPasword(model.NewPassword, out byte[] salt);
                person.PasswordSalt = salt;

                var updated = await _peoplesRepository.ChangePassword(person);
                if (updated != null)
                {
                    Log.Information($"Password of {updated.Username} updated successfully");
                    return _mapper.Map<Person>(updated);
                }

                Log.Information($"Project manager {person.Username} could not be updated");
                throw new CustomException($"Project manager could not be updated");
            });
        }
        /// <summary>
        /// Merr nje person ne baze te adreses se tij email.
        /// </summary>
        /// <param name="email">Email qe sherben per te identifikuar personin.</param>
        /// <returns>Personin perkates.</returns>
        public async Task<Person> GetPersonByEmail(string email)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var person = await _peoplesRepository.GetPersonByEmail(email);
                if (person != null)
                {
                    Log.Information($"Person with username {person.Username} retrieved successfully");
                    return _mapper.Map<DTO.Person>(person);
                }
                Log.Information("Person could not be retrieved");
                throw new CustomException($"Person not found");

            });
        }
        /// <summary>
        /// Merr nje person ne baze te username-it te tij.
        /// </summary>
        /// <param name="username">Username qe sherben per te identifikuar personin.</param>
        /// <returns>Personin perkates.</returns>
        public async Task<Person> GetPersonByUsername(string username)
        {
            return await ServiceExceptionHandler.HandleExceptionAsync(async () =>
            {
                var person = await _peoplesRepository.GetPersonByUsername(username);
                if (person != null)
                {
                    Log.Information($"Person with username {person.Username} retrieved successfully");
                    return _mapper.Map<DTO.Person>(person);
                }

                Log.Error("Person could not be retrieved");
                throw new CustomException($"Person not found");

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
                    Log.Information($"Person with username {person.Username} retrieved successfully");
                    return _mapper.Map<DTO.Person>(person);
                }

                Log.Error("Person could not be retrieved");
                throw new CustomException($"Person not found");

            });
        }
    }
}
