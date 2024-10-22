using BillingApplication.Logic.TariffManager;
using BillingApplication.Models;
using BillingApplication.Server.Logic.Auth.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class TariffController : ControllerBase
    {
        private readonly ITariffManager tariffManager;

        public TariffController(ITariffManager tariffManager)
        {
            this.tariffManager = tariffManager;
        }

        [Authorize(Roles = UserRoles.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Tariff tariffModel)
        {
            var result = await tariffManager.CreateTariff(tariffModel);
            return Ok(result);
        }

        [Authorize(Roles = UserRoles.ADMIN)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Tariff tariffModel)
        {
            var result = await tariffManager.UpdateTariff(tariffModel);
            return Ok(result);
        }

        [Authorize(Roles = UserRoles.ADMIN)]
        [HttpDelete("deletebytitle/{title}")]
        public async Task<IActionResult> DeleteById(string title)
        {
            var result = await tariffManager.DeleteTariff(title);
            return Ok($"Тариф {result} был удалён");
        }

        [Authorize(Roles = UserRoles.ADMIN)]
        [HttpDelete("deletebyid/{id}")]
        public async Task<IActionResult> DeleteByIdTitle(int id)
        {
            var result = await tariffManager.DeleteTariff(id);
            return Ok($"Тариф {result} был удалён");
        }

        [Authorize(Roles = $"{UserRoles.ADMIN}, {UserRoles.OPERATOR}")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await tariffManager.GetAllTariffs();
            return Ok(result);
        }

        [Authorize(Roles = $"{UserRoles.ADMIN}, {UserRoles.OPERATOR}")]
        [HttpGet("getbytitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var result = await tariffManager.GetTariffByTitle(title);
            return Ok(result);
        }

        [Authorize(Roles = $"{UserRoles.ADMIN}, {UserRoles.OPERATOR}")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await tariffManager.GetTariffById(id);
            return Ok(result);
        }
    }
}
