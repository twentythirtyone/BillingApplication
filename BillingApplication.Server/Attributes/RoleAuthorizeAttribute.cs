﻿using Microsoft.AspNetCore.Authorization;

namespace BillingApplication.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class RoleAuthorizeAttribute : Attribute
    {
        public string[] Roles { get; }

        public RoleAuthorizeAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }

}
