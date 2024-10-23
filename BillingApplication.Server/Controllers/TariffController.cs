using BillingApplication.Services.TariffManager;
using BillingApplication.Services.Auth.Roles;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[ServiceFilter(typeof(RoleAuthorizeFilter))]
    public class TariffController : ControllerBase
    {
        private readonly ITariffManager tariffManager;

        public TariffController(ITariffManager tariffManager)
        {
            this.tariffManager = tariffManager;
        }

        //[RoleAuthorize(UserRoles.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] TariffCreateUpdateModel tariffModel)
        {
            try
            {
                var result = await tariffManager.CreateTariff(tariffModel.Tariff, tariffModel.BundleId);
                return Ok(result);
            }
            catch(Exception ex) when(ex is TariffNotFoundException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] TariffCreateUpdateModel tariffModel)
        {
            try
            {
                var result = await tariffManager.UpdateTariff(tariffModel.Tariff, tariffModel.BundleId);
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpDelete("deletebytitle/{title}")]
        public async Task<IActionResult> DeleteByTitle(string title)
        {
            try
            {
                var result = await tariffManager.DeleteTariff(title);
                return Ok($"Тариф {result} был удалён");
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpDelete("deletebyid/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var result = await tariffManager.DeleteTariff(id);
                return Ok($"Тариф {result} был удалён");
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await tariffManager.GetAllTariffs();
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("getbytitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            try
            {
                var result = await tariffManager.GetTariffByTitle(title);
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await tariffManager.GetTariffById(id);
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("getbyuser/{id}")]
        public async Task<IActionResult> GetByUser(int id)
        {
            try
            {
                var result = await tariffManager.GetTariffBySubscriberId(id);
                return Ok(result);
            }
            catch (Exception ex) when (ex is TariffNotFoundException || ex is UserNotFoundException)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
