using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaskManagementSystem.Middlewares
{
    public class AuthenticationMiddleware : IMiddleware
    {
        /// <summary>
        /// Ben middleware te kryeje autentifikimin dhe autorizimin.
        /// </summary>
        /// <param name="context">Konteksti i kerkeses se bere.</param>
        /// <param name="next">Delegati qe perfaqeson middleware-in e rradhes ne pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            if (AllowsAnonymous(context))
            {
                await next(context);
                return;
            }

            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Invalid token.");
                return;
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = "!SomethingSecretAndLongEnoughToBeUseful!";
            var tokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "apiWithAuthBackend",
                ValidAudience = "apiWithAuthBackend",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                var tokenTypeClaim = claimsPrincipal.FindFirst("TokenType");
                if (tokenTypeClaim == null || tokenTypeClaim.Value != "Access")
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Invalid access token.");
                    return;
                }
                context.User = claimsPrincipal;
            }
            catch (Exception)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Invalid token.");
                return;
            }

            await next(context);
        }
        /// <summary>
        /// Kontrollon nese perdoruesit e paautorizuar mund ta aksesojne nje action method.
        /// </summary>
        /// <param name="context">Konteksti qe duhet verifikuar.</param>
        /// <returns>True nese e lejon aksesin e paautorizar, perndryshe false.</returns>
        private bool AllowsAnonymous(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var attributes = endpoint.Metadata.GetOrderedMetadata<AllowAnonymousAttribute>();
                return attributes.Any();
            }
            return false;
        }
    }
}
