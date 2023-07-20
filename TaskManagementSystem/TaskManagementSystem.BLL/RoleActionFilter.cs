using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Configuration;
using System.Security.Claims;

namespace TaskManagementSystem.BLL
{
    public class RoleActionFilter : IActionFilter
    {
        public readonly string[] _allowedRoles;

        public RoleActionFilter(string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }
        /// <summary>
        /// Para se te ekzekutohet nje metode ne controller, kontrollon nese perdoruesi qe po e akseson ka rolin e duhur.
        /// </summary>
        /// <param name="context">Konteksti i ekzekutimit te metodes.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var roleClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                var userRole = roleClaim?.Value;

                if (!_allowedRoles.Contains(userRole))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
