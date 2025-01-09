using BusinessLogic.Services;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoManagementCenter.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/crypto")]
    public class BinanceController : Controller
    {
        private readonly IBinanceService _binanceService;
        public BinanceController(IBinanceService binanceService)
        {
            _binanceService = binanceService;
        }

        [HttpGet("linechart")]
        public async Task<IActionResult> GetLineChartData(string symbol = "BTCUSDT", string interval = "1h")
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return BadRequest("Invalid symbol");
            }

            var data = await _binanceService.GetLineChartPointsAsync(symbol, interval);

            if (data == null)
            {
                return NoContent();
            }

            return Ok(data);
        }
    }
}
