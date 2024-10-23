using BillingApplication.Models;
using BillingApplication.Server.Services.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Services.Auth
{
    public interface IAuth
    {
        public string GenerateJwtToken<T>(T user);
    }
}
