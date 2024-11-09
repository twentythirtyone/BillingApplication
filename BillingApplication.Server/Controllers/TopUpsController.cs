using BillingApplication.Attributes;
using BillingApplication.Server.Services.Manager.TopUpsManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(RoleAuthorizeFilter))]
    public class TopUpsController : ControllerBase
    {
        public readonly ITopUpsManager topUpsManager;
        public TopUpsController(ITopUpsManager TopUpsManager)
        {
            this.topUpsManager = TopUpsManager;
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("add")]
        public async Task<IActionResult> AddTopUp([FromBody] TopUps model)
        {
            try
            {
                await topUpsManager.AddTopUp(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("get")]
        public async Task<IActionResult> GetTopUps()
        {
            try
            {
                var result = await topUpsManager.GetTopUps();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await topUpsManager.GetTopUpById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("getbyuserid/{id}")]
        public async Task<IActionResult> GetTopUpsByUserId(int id)
        {
            try
            {
                var result = await topUpsManager.GetTopUpsByUserId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("getlastbyuserid/{id}")]
        public async Task<IActionResult> GetLastTopUpByUserId(int id)
        {
            try
            {
                var result = await topUpsManager.GetLastTopUpByUserId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
