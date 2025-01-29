using BusinessLogic.Constants;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class MarketDepthService : WebSocketService
    {
        private readonly string[] _symbols = BinanceConstants.MarketDepth.Symbols;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly List<string> savedSymbols = new List<string>();

        public MarketDepthService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task StartConnectionLoop()
        {
            try
            {
                await Task.Delay(2000);

                var task = Task.Run(async () =>
                {
                    var combinedStreamsArray = _symbols.Select(symbol => $"{symbol}@depth5").ToArray();
                    var combinedStreams = string.Join("/", combinedStreamsArray);
                    var url = $"wss://stream.binance.com:9443/stream?streams={combinedStreams}";

                    await ConnectToWebSocket(url);

                    Console.WriteLine($"WebSocket dla market depth został zamknięty. Restart...");
                }, _cancellationTokenSource.Token);

                await task;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas łączenia z WebSocket dla market depth: {ex.Message}");
            }

        }

        public override async Task ProcessMessage(string message)
        {


            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var marketDepthRepository = scope.ServiceProvider.GetRequiredService<IMarketDepthRepository>();

                try
                {
                    var json = JsonDocument.Parse(message);
                    var stream = json.RootElement.GetProperty("stream").GetString();
                    //symbol kryptowaluty. przez to że zajmuję się 4 tj. BTC,ETH, XRP, SOL to biorę 3 pierwsze znaki z stream
                    string symbol = stream.Substring(0, 3).ToUpper();
                    string bids = json.RootElement.GetProperty("data").GetProperty("bids").ToString();
                    string asks = json.RootElement.GetProperty("data").GetProperty("asks").ToString();
                    long timeStampMs = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

                    var marketDepth = new MarketDepthModel
                    {
                        Symbol = symbol,
                        Bids = bids,
                        Asks = asks,
                        TimeStampMs = timeStampMs
                    };

                    if(!savedSymbols.Contains(symbol))
                    {
                        await marketDepthRepository.AddMarketDepthAsync(marketDepth);
                    }

                    savedSymbols.Add(symbol);
                    
                    if(savedSymbols.Count == 4)
                    {
                        await Task.Delay(60000);
                        savedSymbols.Clear();
                    }

                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Błąd przetwarzania danych: {ex.Message}");
                }
            }
        }
    }
}
