using BillingApplication.Attributes;
using BillingApplication.Server.Controllers;
using BillingApplication.Server.Services.Manager.ExtrasManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Utilites;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Controllers
{
    [Route("extras")]
    [ApiController]
    public class ExtrasController : ControllerBase
    {
        private readonly IExtrasManager extrasManager;
        private readonly ILogger<ExtrasController> logger;
        public ExtrasController(IExtrasManager extrasManager, ILogger<ExtrasController> logger)
        {
            this.extrasManager = extrasManager;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetExtras()
        {
            var result = await extrasManager.GetExtras();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExtras(int id)
        {
            var result = await extrasManager.GetExtrasById(id);
            return Ok(result);
        }

        [HttpPost("new")]
        [RoleAuthorize(UserRoles.ADMIN)]
        public async Task<IActionResult> Create([FromBody] Extras extrasModel)
        {
            var result = await extrasManager.AddNewExtra(extrasModel);
            return Ok(result);
        }
    }
}
