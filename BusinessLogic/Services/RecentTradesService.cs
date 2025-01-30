using BusinessLogic.Constants;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class RecentTradesService : WebSocketService
    {
        private readonly string[] _symbols = BinanceConstants.MarketDepth.Symbols;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RecentTradesService(IServiceScopeFactory serviceScopeFactory)
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
                    var combinedStreamsArray = _symbols.Select(symbol => $"{symbol}@trade");
                    var combinedStreams = string.Join("/", combinedStreamsArray);
                    var url = $"wss://stream.binance.com:9443/stream?streams={combinedStreams}";

                    await ConnectToWebSocket(url);

                    Console.WriteLine($"WebSocket dla recent trades został zamknięty. Restart...");
                },_cancellationTokenSource.Token);

                await task;
            }catch(Exception ex)
            {
                Console.WriteLine($"Błąd podczas łączenia z WebSocket dla recent trades: {ex.Message}");
            }
        }

        public override async Task ProcessMessage(string message)
        {
            var elo = message;
        }
    }
}
