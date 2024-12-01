using BillingApplication.Services.Auth.Roles;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Services.Models.Utilites.Tariff;
using BillingApplication.Server.Services.Manager.TariffManager;
using System.Text.Json;

namespace BillingApplication.Controllers
{
    [Route("tariff")]
    [ApiController]
    [ServiceFilter(typeof(RoleAuthorizeFilter))]
    public class TariffController : ControllerBase
    {
        private readonly ITariffManager tariffManager;
        private readonly ILogger<SubscriberController> logger;

        public TariffController(ITariffManager tariffManager, ILogger<SubscriberController> logger)
        {
            this.tariffManager = tariffManager;
            this.logger = logger;
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] TariffCreateUpdateModel tariffModel)
        {
            try
            {
                var result = await tariffManager.CreateTariff(tariffModel.Tariff, tariffModel.BundleId);
                logger.LogInformation($"ADDING: Tariff {tariffModel.Tariff.Id} with Bundle {tariffModel.BundleId} added");
                return Ok(result);
            }
            catch(Exception ex) when(ex is TariffNotFoundException || ex is InvalidOperationException)
            {
                logger.LogError($"ERROR ADDING: Tariff {tariffModel.Tariff.Id} with Bundle {tariffModel.BundleId} has not been added" +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nModel: {JsonSerializer.Serialize(tariffModel)}\n");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] TariffCreateUpdateModel tariffModel)
        {
            try
            {
                var result = await tariffManager.UpdateTariff(tariffModel.Tariff, tariffModel.BundleId);
                logger.LogInformation($"UPDATE: Tariff {tariffModel.Tariff.Id} has been updated");
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR UPDATE: Tariff {tariffModel.Tariff.Id} has not been updated" +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nModel: {JsonSerializer.Serialize(tariffModel)}\n");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpDelete("{title}/delete")]
        public async Task<IActionResult> DeleteByTitle(string title)
        {
            try
            {
                var result = await tariffManager.DeleteTariff(title);
                logger.LogInformation($"DELETE: Tariff {result} has been deleted");
                return Ok($"Тариф {result} был удалён");
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR DELETE: Tariff has not been deleted" +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nModel: title: {title}\n");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var result = await tariffManager.DeleteTariff(id);
                logger.LogInformation($"DELETE: Tariff {result} has been deleted");
                return Ok($"Тариф {result} был удалён");
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR DELETE: Tariff has not been deleted" +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nModel: id: {id}\n");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await tariffManager.GetAllTariffs();
                logger.LogInformation($"GETTING: Tariffs has been recieved");
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Tariffs has not been recieved" +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            try
            {
                var result = await tariffManager.GetTariffByTitle(title);
                logger.LogInformation($"GETTING: Tariff {result} has been recieved");
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Tariff has not been recieved" +
                                     $"\nMessage:{ex.Message}" +
                                     $"\nModel: title: {title}");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await tariffManager.GetTariffById(id);
                logger.LogInformation($"GETTING: Tariff {result} has been recieved");
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Tariff has not been recieved" +
                     $"\nMessage:{ex.Message}" +
                     $"\nModel: id: {id}");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            try
            {
                var result = await tariffManager.GetTariffBySubscriberId(userId);
                logger.LogInformation($"GETTING: Tariff {result} has been recieved");
                return Ok(result);
            }
            catch (Exception ex) when (ex is TariffNotFoundException || ex is UserNotFoundException)
            {
                logger.LogError($"ERROR GETTING: Tariff has not been recieved" +
                     $"\nMessage:{ex.Message}" +
                     $"\nModel: userId: {userId}");
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{id}/bundle")]
        public async Task<IActionResult> GetBundleByTariffId(int id)
        {
            try
            {
                var result = await tariffManager.GetBundleByTariffId(id);
                logger.LogInformation($"GETTING: Bundle {result} from tariff {id} has been recieved");
                return Ok(result);
            }
            catch (Exception ex) when (ex is TariffNotFoundException)
            {
                logger.LogError($"ERROR GETTING: Bundle has not been recieved" +
                     $"\nMessage:{ex.Message}" +
                     $"\nModel: tariffId: {id}");
                return BadRequest(ex.Message);
            }
        }
    }
}
