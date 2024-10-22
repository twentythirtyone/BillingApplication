using BillingApplication.Logic.TariffManager;
using BillingApplication.Models;
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

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Tariff tariffModel)
        {
            var result = await tariffManager.CreateTariff(tariffModel);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Tariff tariffModel)
        {
            var result = await tariffManager.UpdateTariff(tariffModel);
            return Ok(result);
        }

        [HttpDelete("deletebytitle/{title}")]
        public async Task<IActionResult> DeleteById(string title)
        {
            var result = await tariffManager.DeleteTariff(title);
            return Ok($"Тариф {result} был удалён");
        }

        [HttpDelete("deletebyid/{id}")]
        public async Task<IActionResult> DeleteByIdTitle(int id)
        {
            var result = await tariffManager.DeleteTariff(id);
            return Ok($"Тариф {result} был удалён");
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await tariffManager.GetAllTariffs();
            return Ok(result);
        }

        [HttpGet("getbytitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var result = await tariffManager.GetTariffByTitle(title);
            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await tariffManager.GetTariffById(id);
            return Ok(result);
        }
    }
}
