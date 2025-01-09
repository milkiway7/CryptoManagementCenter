using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IBinanceService
    {
        Task<List<LineChartPoint>> GetLineChartPointsAsync(string symbol, string interval);
    }
}
