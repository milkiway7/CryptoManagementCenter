using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class CandleWebSocketService : WebSocketService
    {
        private readonly string[] _symbols = { "btcusdt", "ethusdt", "xrpusdt", "solusdt" };
        private readonly string[] _intervals = { "1m", "5m", "15m", "1h" };
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CandleWebSocketService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task ProcessMessage(string message)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var candleRepository = scope.ServiceProvider.GetRequiredService<ICandleRepository>();

                try
                {
                    var json = JsonDocument.Parse(message);
                    var stream = json.RootElement.GetProperty("stream").GetString();
                    var kline = json.RootElement.GetProperty("data").GetProperty("k");
                    var symbol = stream.Substring(0,3).ToUpper(); 
                    var interval = stream.Split('@')[1].Split('_')[1]; 
                    var isCandleClosed = kline.GetProperty("x").GetBoolean();

                    if(!isCandleClosed)
                    {
                        return;
                    }

                    var candle = new CandleModel
                    {
                        Symbol = symbol,
                        Interval = interval, 
                        OpenTime = long.Parse(kline.GetProperty("t").ToString()),  
                        Open = decimal.Parse(kline.GetProperty("o").GetString(), CultureInfo.InvariantCulture),
                        High = decimal.Parse(kline.GetProperty("h").GetString(), CultureInfo.InvariantCulture),
                        Low = decimal.Parse(kline.GetProperty("l").GetString(), CultureInfo.InvariantCulture),
                        Close = decimal.Parse(kline.GetProperty("c").GetString(), CultureInfo.InvariantCulture),
                        Volume = decimal.Parse(kline.GetProperty("v").GetString(), CultureInfo.InvariantCulture)
                    };

                    await candleRepository.AddCandleAsync(candle);

                    Console.WriteLine($"Zapisano świecę dla {symbol} {interval}: {candle.OpenTime} | Open: {candle.Open}, Close: {candle.Close}");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd przetwarzania danych: {ex.Message}");
                }
            }
        }

        protected override async Task StartConnectionLoop()
        {
            var tasks = _symbols.Select(symbol => Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        var streamNames = _intervals.Select(interval => $"{symbol}@kline_{interval}").ToArray();
                        var combinedStreams = string.Join("/", streamNames);
                        var wsUrl = $"wss://stream.binance.com:9443/stream?streams={combinedStreams}";

                        Console.WriteLine($"Uruchamianie WebSocket dla {symbol}");
                        await ConnectToWebSocket(wsUrl);

                        Console.WriteLine($"WebSocket dla {symbol} został zamknięty. Restart...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Błąd WebSocket dla candles {symbol}: {ex.Message}");
                    }

                    // Jeśli WebSocket się zamknie, czekamy kilka sekund przed ponownym połączeniem, żeby nie spamować Binance
                    await Task.Delay(2000);
                }
            }, _cancellationTokenSource.Token)).ToList();

            // Czekamy na wszystkie WebSockety
            await Task.WhenAll(tasks);
        }
    }
}
