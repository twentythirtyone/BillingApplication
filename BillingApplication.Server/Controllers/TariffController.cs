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

        //TODO: методы редактирования, удаления
    }
}
