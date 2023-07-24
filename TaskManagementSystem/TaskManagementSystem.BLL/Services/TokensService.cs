using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BLL.DTO;
using TaskManagementSystem.BLL.Interfaces;
using TaskManagementSystem.Common;
using TaskManagementSystem.Common.CustomExceptions;
using TaskManagementSystem.DAL.Interfaces;

namespace TaskManagementSystem.BLL.Services
{
    internal class TokensService:ITokensService
    {

        private readonly IAuthenticationsRepository _authenticationsRepository;
        private readonly IMapper _mapper;
        public TokensService(IAuthenticationsRepository authenticationsRepository, IMapper mapper)
        {
            _authenticationsRepository = authenticationsRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Krijon nje token per nje perdorues te caktuar.
        /// </summary>
        /// <param name="user">Perdoruesi i cili dot e pajiset me token.</param>
        /// <returns>Token se basku me nje refresh token per te shmangur dhenien e perseritur te kredencialeve. </returns>
        public TokenResponse CreateToken(DAL.Entities.Person user)
        {

            var expiration = DateTime.UtcNow.AddMinutes(30);
            var accessClaims = CreateClaims(user);
            accessClaims.Add(new Claim("TokenType", "Access"));
            var token = CreateJwtToken(
                accessClaims,
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            var refreshToken = CreateRefreshToken(user);
            

            return new TokenResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken,
            };

        }
        /// <summary>
        /// Krijon nje refresh token per nje perdorues te caktuar.
        /// </summary>
        /// <param name="user">Perdoruesi i cili dot e pajiset me refresh token.</param>
        /// <returns>Refresh token-in prkates.</returns>
        private string CreateRefreshToken(DAL.Entities.Person user)
        {
            var expiration = DateTime.UtcNow.AddDays(7);
            var refreshToken = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(refreshToken);
        }
        /// <summary>
        /// Krijon nje token me te drejtat e caktuara, kredencialet, dhe kohen e skadimit.
        /// </summary>
        /// <param name="claims">Lista e te drejtave te token-it.</param>
        /// <param name="credentials">Kredencilaet e token.</param>
        /// <param name="expiration">Koha e skadimit e token.</param>
        /// <returns>Token-in e krijuar.</returns> 
        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) =>
            new(
                "apiWithAuthBackend",
                "apiWithAuthBackend",
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        /// <summary>
        /// Krijon listen e te drejtave bazuar ne perdoruesin perkates.
        /// </summary>
        /// <param name="user">Perdoruesi perkates.</param>
        /// <returns>Listen e te drejtave te perdoruesit.</returns>
        private List<Claim> CreateClaims(DAL.Entities.Person user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.PersonType.ToString())
                };
                return claims;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// Kriijon kredencialet e token-it.
        /// </summary>
        /// <returns>Kredencialet e token-it.</returns>
        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("!SomethingSecretAndLongEnoughToBeUseful!")
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
        /// <summary>
        /// Ben refresh token-in e saposkaduar duke perdorur nje refresh token.
        /// </summary>
        /// <param name="refreshToken">Refresh token qe i eshte bashkangjitur token-it te saposkaduar 
        /// ne momentin e krijimit te tij.</param>
        /// <returns>TokenResponse qe permban nje token te ri dhe nje refresh token te ri.</returns>
        public async Task<TokenResponse> RefreshToken(string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(refreshToken);

            if (principal == null)
            {
                throw new CustomException("Invalid refresh token");
            }

            Int32.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId);

            var user = await _authenticationsRepository.GetPersonById(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var accessToken = CreateToken(_mapper.Map<DAL.Entities.Person>(user));

            return new TokenResponse
            {
                AccessToken = accessToken.AccessToken,
                RefreshToken = accessToken.RefreshToken,
            };
        }
        /// <summary>
        /// Merr te drejtat e token-it te saposkaduar.
        /// </summary>
        /// <param name="token">Token-i i saposkaduar.</param>
        /// <returns>Te drejtat e token-it.</returns>
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "apiWithAuthBackend",
                ValidAudience = "apiWithAuthBackend",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!SomethingSecretAndLongEnoughToBeUseful!"))
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
