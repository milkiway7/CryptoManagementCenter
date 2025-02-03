using BusinessLogic.Constants;
using BusinessLogic.Models;
using DataAccess.Models;
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
    public class RecentTradesService : WebSocketService
    {
        private readonly string[] _symbols = BinanceConstants.MarketDepth.Symbols;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        private Dictionary<string, List<RecentTradesModel>> _recentTrades = new Dictionary<string, List<RecentTradesModel>>()
            {
                { "BTC", new List<RecentTradesModel>() },
                { "ETH", new List<RecentTradesModel>() },
                { "XRP", new List<RecentTradesModel>() },
                { "SOL", new List<RecentTradesModel>() },
            };

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
            using (IServiceScope scope = _serviceScopeFactory.CreateScope()) {

                var recentTradesRepository = scope.ServiceProvider.GetRequiredService<IRecentTradesRepository>();

                try
                {
                    var json = JsonDocument.Parse(message);
                    var data = json.RootElement.GetProperty("data");

                    RecentTradesModel recentTrade = new RecentTradesModel()
                    {
                        Symbol = json.RootElement.GetProperty("stream").ToString().Substring(0, 3).ToUpper(),
                        TimeStampMs = long.Parse(data.GetProperty("T").ToString()),
                        Price = decimal.Parse(data.GetProperty("p").ToString(), CultureInfo.InvariantCulture),
                        Quantity = decimal.Parse(data.GetProperty("q").ToString(), CultureInfo.InvariantCulture),
                        IsBuyerMaker = bool.Parse(data.GetProperty("m").ToString())
                    };

                    AddTradeToDictionary(recentTrade);

                    if(_timer == null)
                    {
                        _timer = new Timer(async (e) =>
                        {
                            Console.WriteLine("Timer działa: " + DateTime.UtcNow);

                            bool isEmpty = CheckIfEmpty();
                            if (!isEmpty)
                            {
                                using (var newScope = _serviceScopeFactory.CreateScope())
                                {
                                    var newRecentTradesRepository = newScope.ServiceProvider.GetRequiredService<IRecentTradesRepository>();
                                    await SaveTradesToDatabase(newRecentTradesRepository);
                                }

                                foreach (var key in _recentTrades.Keys)
                                {
                                    _recentTrades[key].Clear();
                                }
                            }

                        }, null, 6000 , 60000);
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas przetwarzania wiadomości dla recent trades: {ex.Message}");
                }
            }
        }

        private void AddTradeToDictionary(RecentTradesModel recentTrade)
        {
            if (_recentTrades.ContainsKey(recentTrade.Symbol))
            {
                if (_recentTrades[recentTrade.Symbol].Count == 0)
                {
                    _recentTrades[recentTrade.Symbol].Add(recentTrade);
                }
                else
                {
                    var quantity = _recentTrades[recentTrade.Symbol][0].Quantity;
                    if (recentTrade.Quantity > quantity )
                    {
                        _recentTrades[recentTrade.Symbol][0] = recentTrade;
                    }
                }

            }
        }

        private async Task SaveTradesToDatabase(IRecentTradesRepository recentRepository)
        {
            foreach(var key in _recentTrades)
            {
                await recentRepository.AddRecentTradesAsync(_recentTrades[key.Key].First());
            }
        }

        private bool CheckIfEmpty()
        {
            bool isEmpty = true;

            foreach (var key in _recentTrades)
            {
                if (_recentTrades[key.Key].Count > 0)
                {
                    isEmpty = false;
                }
                else
                {
                    isEmpty = true;
                    break;
                }
            }

            return isEmpty;
        }
    }
}
