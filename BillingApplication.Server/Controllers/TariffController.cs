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

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] Tariff tariffModel)
        {
            var result = await tariffManager.CreateTariff(tariffModel);
            return Ok(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await tariffManager.GetAllTariffs();
            return Ok(result);
        }

        [HttpGet("getbytitle")]
        public async Task<IActionResult> GetByTitle([FromBody] string title)
        {
            var result = await tariffManager.GetTariffByTitle(title);
            return Ok(result);
        }
    }
}
