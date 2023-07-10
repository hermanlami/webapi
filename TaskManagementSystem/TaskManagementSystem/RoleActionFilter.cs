using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Configuration;
using System.Security.Claims;

public class RoleActionFilter : IActionFilter
{
    public readonly string[] _allowedRoles;

    public RoleActionFilter(string[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }
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
