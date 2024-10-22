using BillingApplication.Logic.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IAuth auth;
        public UserController()
        {
            
        }
    }
}
