using BillingApplication.Attributes;
using BillingApplication.Controllers;
using BillingApplication.Server.Services.Manager.TopUpsManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BillingApplication.Server.Controllers
{
    [Route("topups")]
    [ApiController]
    [ServiceFilter(typeof(RoleAuthorizeFilter))]
    public class TopUpsController : ControllerBase
    {
        public readonly ITopUpsManager topUpsManager;
        public readonly ILogger<TopUpsController> logger;
        public TopUpsController(ITopUpsManager topUpsManager, ILogger<TopUpsController> logger)
        {
            this.topUpsManager = topUpsManager;
            this.logger = logger;
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] TopUps model)
        {
            try
            {
                await topUpsManager.AddTopUp(model);
                logger.LogInformation($"ADDING: TopUp {model.Id} added");
                return Ok(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR ADDING: TopUp {model.Id} has not been added" +
                                $"\nMessage:{ex.Message}" +
                                $"\nModel: {JsonSerializer.Serialize(model)}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await topUpsManager.GetTopUps();
                logger.LogInformation($"GETTING: TopUps recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: TopUps has not been recieved" +
                                $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await topUpsManager.GetTopUpById(id);
                logger.LogInformation($"GETTING: TopUp {id} recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: TopUp {id} has not been recieved" +
                                $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/topups/user/id/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                var result = await topUpsManager.GetTopUpsByUserId(id);
                logger.LogInformation($"GETTING: User's {id} TopUps recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: User's {id} TopUps has not been recieved" +
                                $"\nMessage:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/topups/last/user/id/{id}")]
        public async Task<IActionResult> GetLastByUserId(int id)
        {
            try
            {
                var result = await topUpsManager.GetLastTopUpByUserId(id);
                logger.LogInformation($"GETTING: User's {id} last TopUp recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: User's {id} last TopUp has not been recieved" +
                $"\nMessage:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
