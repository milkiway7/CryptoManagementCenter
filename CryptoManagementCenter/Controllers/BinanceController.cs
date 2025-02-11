﻿using BusinessLogic.Models;
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
        public async Task<IActionResult> GetLineChartData([FromQuery] string symbol, [FromQuery] string interval, [FromQuery] long? startTime)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return BadRequest("Invalid symbol");
            }

            symbol += "USDT";

            var data = await _binanceService.GetLineChartPointsAsync(symbol, interval, startTime);

            if (data == null)
            {
                return NoContent();
            }

            return Ok(data);
        }

        [HttpGet("trades")]
        public async Task<IActionResult> GetTradesData([FromQuery] string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return BadRequest("Invalid symbol"); 
            }

            List<RecentTradeModel> recentTrades = await _binanceService.GetRecentTradesAsync(symbol);

            if (recentTrades == null)
            {
                return NoContent();
            }

            return Ok(recentTrades);
        }
    }
}
