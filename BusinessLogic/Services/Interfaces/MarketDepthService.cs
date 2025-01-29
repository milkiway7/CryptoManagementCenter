using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public class MarketDepthService : WebSocketService
    {
        private readonly string[] _symbols = { "btcusdt", "ethusdt", "xrpusdt", "solusdt" };
        private readonly IServiceScopeFactory _serviceScopeFactory;

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
                },_cancellationTokenSource.Token);

                await task;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas łączenia z WebSocket dla market depth: {ex.Message}");
            }

        }

        public override async Task ProcessMessage(string message)
        {
            var lolo = message;
        }
    }
}
