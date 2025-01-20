using BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public abstract class WebSocketService : IWebSocketService
    {
        protected readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private const int MaxReconnectAttempts = 5;

        protected WebSocketService()
        {}

        // Rozpoczęcie połączenia
        public async Task StartAsync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                await StartConnectionLoop();
            }
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
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            Console.WriteLine("WebSocket został zatrzymany.");
        }
    }
}
