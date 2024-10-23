using BillingApplication.Services.TariffManager;
using BillingApplication.Models;
using BillingApplication.Server.Services.Auth.Roles;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Server.Attributes;
using BillingApplication.Server.Exceptions;

namespace BillingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(RoleAuthorizeFilter))]
    public class TariffController : ControllerBase
    {
        private readonly ITariffManager tariffManager;

        public TariffController(ITariffManager tariffManager)
        {
            this.tariffManager = tariffManager;
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Tariff tariffModel)
        {
            try
            {
                var result = await tariffManager.CreateTariff(tariffModel);
                return Ok(result);
            }
            catch(TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Tariff tariffModel)
        {
            try
            {
                var result = await tariffManager.UpdateTariff(tariffModel);
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpDelete("deletebytitle/{title}")]
        public async Task<IActionResult> DeleteById(string title)
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
        public async Task<IActionResult> DeleteByIdTitle(int id)
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
    }
}
