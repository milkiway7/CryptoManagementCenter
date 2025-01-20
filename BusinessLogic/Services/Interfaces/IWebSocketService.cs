using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IWebSocketService
    {
        Task StartAsync();
        void Stop();
        Task ProcessMessage(string message);
    }
}
