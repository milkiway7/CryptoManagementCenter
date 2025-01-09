using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class BinanceService : IBinanceService
    {
        private readonly HttpClient _httpClient;

        public BinanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<LineChartPoint>> GetLineChartPointsAsync(string symbol, string interval)
        {
            var response = await _httpClient.GetAsync($"https://api.binance.com/api/v3/klines?symbol={symbol}&interval={interval}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            List<List<JsonElement>> klineData = JsonSerializer.Deserialize<List<List<JsonElement>>>(jsonResponse);

            List<LineChartPoint> lineChartData = new List<LineChartPoint>();

            foreach(List<JsonElement> data in klineData)
            {
                long closingTimeUnixSeconds = long.Parse(data[6].GetRawText()) / 1000;
                decimal closingPrice = decimal.Parse(data[4].GetString(), CultureInfo.InvariantCulture);
                var a = DateTimeOffset.FromUnixTimeSeconds(closingTimeUnixSeconds).LocalDateTime;

                lineChartData.Add(new LineChartPoint {
                    ClosingTimeUnixSeconds = closingTimeUnixSeconds,
                    ClosingTime = DateTimeOffset.FromUnixTimeSeconds(closingTimeUnixSeconds).LocalDateTime,
                    Price = closingPrice,
                });
            }

            //var klineData = JsonSerializer.Deserialize<L>
            return lineChartData;
        }
    }
}
