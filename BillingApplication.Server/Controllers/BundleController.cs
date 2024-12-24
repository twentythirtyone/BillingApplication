using BillingApplication.Attributes;
using BillingApplication.Controllers;
using BillingApplication.Exceptions;
using BillingApplication.Server.Exceptions;
using BillingApplication.Server.Services.Manager.BundleManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Utilites;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace BillingApplication.Server.Controllers
{
    [Route("bundle")]
    [ApiController]
    [ServiceFilter(typeof(RoleAuthorizeFilter))]
    public class BundleController : ControllerBase
    {
        private readonly IBundleManager bundleManager;
        private readonly ILogger<BundleController> logger;

        public BundleController(IBundleManager bundleManager, ILogger<BundleController> logger)
        {
            this.bundleManager = bundleManager;
            this.logger = logger;
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Bundle BundleModel)
        {
            try
            {
                var result = await bundleManager.CreateBundle(BundleModel);
                logger.LogInformation($"ADDING: new Bundle has been added: \nModel: {JsonSerializer.Serialize(BundleModel)}\n");
                return Ok(result);
            }
            catch (BundleNotFoundException ex)
            {
                logger.LogError($"ERROR ADDING: new Bundle has not been added." +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nModel: {JsonSerializer.Serialize(BundleModel)}\n");

                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] Bundle BundleModel)
        {
            try
            {
                var result = await bundleManager.UpdateBundle(BundleModel);
                logger.LogInformation($"EDITING: bundle {result} has been edited");
                return Ok(result);
            }
            catch (BundleNotFoundException ex)
            {
                logger.LogError($"ERROR EDITING: Bundle has not been edited." +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nModel: {JsonSerializer.Serialize(BundleModel)}\n");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var result = await bundleManager.DeleteBundle(id);
                logger.LogInformation($"DELETE: Bundle {id} has been deleted");
                return Ok($"Пакет {result} был удалён");
            }
            catch (BundleNotFoundException ex)
            {
                logger.LogError($"ERROR DELETE: Bundle has not been deleted." +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nId: {id}\n");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await bundleManager.GetAllBundles();
                logger.LogInformation($"GETTING: Bundles has been recieved");
                return Ok(result);
            }
            catch (BundleNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Bundles has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }


        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await bundleManager.GetBundleById(id);
                logger.LogInformation($"GETTING: Bundle {id} has been recieved");
                return Ok(result);
            }
            catch (BundleNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Bundle {id} has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }
    }
}
