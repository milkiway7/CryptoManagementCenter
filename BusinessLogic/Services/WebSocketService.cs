using BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public abstract class WebSocketService : IWebSocketService, IHostedService
    {
        protected readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private const int MaxReconnectAttempts = 5;
        private Task _backgroundTask;

        protected WebSocketService()
        {}

        // Rozpoczęcie połączenia
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _backgroundTask = Task.Run(async () => await StartConnectionLoop(), cancellationToken);
            return Task.CompletedTask; // Nie blokujemy aplikacji
        }

        // Metoda abstrakcyjna do połączenia, implementowana w klasach dziedziczących
        protected abstract Task StartConnectionLoop();

        // Łączenie z WebSocket
        protected async Task ConnectToWebSocket(string wsUrl)
        {
            using var client = new ClientWebSocket(); 
            int attemptCount = 0;

            while (attemptCount < MaxReconnectAttempts && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    await client.ConnectAsync(new Uri(wsUrl), CancellationToken.None);
                    Console.WriteLine($"Połączono z WebSocket: {wsUrl}");

                    var buffer = new byte[8192];

                    while (client.State == WebSocketState.Open && !_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        if (!string.IsNullOrEmpty(message))
                        {
                            await ProcessMessage(message);
                        }
                    }

                    break; // Jeśli WebSocket został zamknięty poprawnie, wyjdź z pętli
                }
                catch (WebSocketException ex)
                {
                    Console.WriteLine($"Błąd WebSocket: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }

                attemptCount++;
                if (attemptCount < MaxReconnectAttempts)
                {
                    Console.WriteLine($"Próba ponownego połączenia (Próba {attemptCount}/{MaxReconnectAttempts})...");
                    await Task.Delay(2000); // Czekaj 2 sekundy przed ponowną próbą
                }
            }

            if (attemptCount >= MaxReconnectAttempts)
            {
                Console.WriteLine($"Nie udało się połączyć z WebSocket po {MaxReconnectAttempts} próbach.");
            }

            // **Poprawne zamknięcie WebSocket przed usunięciem**
            if (client.State == WebSocketState.Open)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Zamykanie połączenia", CancellationToken.None);
            }
        }

        // Metoda abstrakcyjna do przetwarzania danych, implementowana w klasach dziedziczących
        public abstract Task ProcessMessage(string message);

        // Zatrzymanie połączenia
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();

            if (_backgroundTask != null)
            {
                try
                {
                    await _backgroundTask; // Czekamy na zakończenie WebSocket
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas zatrzymywania WebSocket: {ex.Message}");
                }
            }

            Console.WriteLine("WebSocket został zatrzymany.");
        }
    }
}
