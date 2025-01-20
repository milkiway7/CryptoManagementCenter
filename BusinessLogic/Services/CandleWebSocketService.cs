using DataAccess.Models;
using System;
using System.Collections.Generic;
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

        public CandleWebSocketService()
        {
        }

        public override Task ProcessMessage(string message)
        {
            try
            {
                var json = JsonDocument.Parse(message);

                // Binance w combined streams zwraca dane w strukturze, gdzie nazwa strumienia jest w "stream"
                var stream = json.RootElement.GetProperty("stream").GetString();
                var kline = json.RootElement.GetProperty("data").GetProperty("k");

                var symbol = stream.Split('@')[0].ToUpper(); // Wyciągamy symbol z nazwy strumienia
                var interval = stream.Split('@')[1].Split('_')[1]; // Wyciągamy interwał z nazwy strumienia

                var candle = new CandleModel
                {
                    Symbol = symbol,
                    Interval = interval, // Przechowujemy interwał
                    OpenTime = kline.GetProperty("t").GetInt64(),  // Przechowujemy jako long
                    Open = kline.GetProperty("o").GetDecimal(),
                    High = kline.GetProperty("h").GetDecimal(),
                    Low = kline.GetProperty("l").GetDecimal(),
                    Close = kline.GetProperty("c").GetDecimal(),
                    Volume = kline.GetProperty("v").GetDecimal()
                };
                // Zapisanie świecy do odpowiedniego repozytorium
                await _candleRepository.AddCandleAsync(candle);
                
                Console.WriteLine($"Zapisano świecę dla {symbol} {interval}: {candle.OpenTime} | Open: {candle.Open}, Close: {candle.Close}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przetwarzania danych: {ex.Message}");
            }

        }

        protected override async Task StartConnectionLoop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                foreach (var symbol in _symbols)
                {
                    // Generowanie URL łączącego wszystkie interesujące interwały
                    var streamNames = _intervals.Select(interval => $"{symbol}@kline_{interval}").ToArray();
                    var combinedStreams = string.Join("/", streamNames);

                    var wsUrl = $"wss://stream.binance.com:9443/stream?streams={combinedStreams}";

                    await ConnectToWebSocket(wsUrl); // Połączenie z WebSocket
                }
            }
        }
    }
}
