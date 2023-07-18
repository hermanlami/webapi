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
