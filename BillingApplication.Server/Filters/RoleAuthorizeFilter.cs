using BillingApplication.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using System.Security.Claims;

public class RoleAuthorizeFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var endpoint = context.HttpContext.GetEndpoint();
        var roleAttribute = endpoint?.Metadata.GetMetadata<RoleAuthorizeAttribute>();

        if (roleAttribute != null)
        {
            var roles = roleAttribute.Roles;

            if (!user.Identity.IsAuthenticated || !roles.Any(role => user.IsInRole(role)))
            {
                context.Result = new ObjectResult(new
                {
                    message = "Unauthorized access. You do not have permission to access this resource."
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized // Код состояния 401
                };
            }
        }
    }
}





