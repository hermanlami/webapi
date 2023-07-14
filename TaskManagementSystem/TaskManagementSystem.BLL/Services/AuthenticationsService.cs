using AutoMapper;
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
using TaskManagementSystem.DAL.Repositories;

namespace TaskManagementSystem.BLL.Services
{
    internal class AuthenticationsService:IAuthenticationsService
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
            try
            {
                var pM = await _peoplesRepository.GetPersonByEmail(request.Email);
                if (pM == null)
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

        public async Task<Person> ChangePassword(int id, UpdatePasswordRequest model)
        {
            try
            {
                var person = await _peoplesRepository.GetPersonById(id);
                if (person  == null)
                {
                    Log.Information("User not found");
                    throw new CustomException($"User not found");
                }
                if(!PasswordHashing.VerifyPassword(model.OldPassword, person.PasswordHash, person.PasswordSalt))
                {
                    Log.Error("Old password value is incorrect");
                    throw new CustomException($"Old password value is incorrect");
                }
                person.PasswordHash = PasswordHashing.HashPasword(model.NewPassword, out byte[] salt);
                person.PasswordSalt = salt;

                var updated = await _peoplesRepository.ChangePassword(person);
                if (updated != null)
                {
                    Log.Information($"Password of user with username {updated.Username} updated successfully");
                    return _mapper.Map<Person>(updated);
                }

                Log.Information($"Project manager with username {person.Username} could not be updated");
                throw new CustomException($"Project manager could not be updated");
            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            try
            {
                var person = await _peoplesRepository.GetPersonByEmail(email);
                if (person != null)
                {
                    Log.Information($"Person with username {person.Username} retrieved successfully");
                    return _mapper.Map<DTO.Person>(person);
                }
                Log.Information("Person could not be retrieved");
            }
            catch (CustomException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Person> GetPersonByUsername(string username)
        {
            try
            {
                var person = await _peoplesRepository.GetPersonByUsername(username);
                if (person != null)
                {
                    Log.Information($"Person with username {person.Username} retrieved successfully");
                    return _mapper.Map<DTO.Person>(person);
                }

                Log.Error("Person could not be retrieved");
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
