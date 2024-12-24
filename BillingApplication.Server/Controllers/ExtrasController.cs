using BillingApplication.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Server.Controllers;
using BillingApplication.Server.Services.Manager.ExtrasManager;
using BillingApplication.Server.Services.Manager.TariffManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Services.Models.Utilites.Tariff;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            logger.LogInformation($"GETTING: Extras has been recieved.");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExtras(int id)
        {
            var result = await extrasManager.GetExtrasById(id);
            logger.LogInformation($"GETTING: Extra {id} has been recieved.");
            return Ok(result);
        }

        [HttpPost("new")]
        [RoleAuthorize(UserRoles.ADMIN)]
        public async Task<IActionResult> Create([FromBody] Extras extrasModel)
        {
            var result = await extrasManager.AddNewExtra(extrasModel);
            logger.LogInformation($"ADDING: new Extra has been added.\nModel: {JsonSerializer.Serialize(extrasModel)}\n");
            return Ok(result);
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] Extras extrasModel, int bundleId)
        {
            try
            {
                var result = await extrasManager.Update(extrasModel, bundleId);
                logger.LogInformation($"UPDATE: Extra {extrasModel.Id} has been updated");
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR UPDATE: Extra {extrasModel.Id} has not been updated" +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nModel: {JsonSerializer.Serialize(extrasModel)}\n");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                var result = await extrasManager.Delete(id);
                logger.LogInformation($"UPDATE: Extra {id} has been deleted");
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR UPDATE: Extra {id} has not been deleted" +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nModel: id: {id}\n");
                return BadRequest(ex.Message);
            }
        }
    }
}
