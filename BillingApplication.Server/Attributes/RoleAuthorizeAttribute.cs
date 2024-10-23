using Microsoft.AspNetCore.Authorization;

namespace BillingApplication.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RoleAuthorizeAttribute : Attribute
    {
        public string[] Roles { get; }

        public RoleAuthorizeAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }

}
