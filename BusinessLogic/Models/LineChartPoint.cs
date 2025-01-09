using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class LineChartPoint
    {
        public long ClosingTimeUnixSeconds {  get; set; }
        public DateTime ClosingTime { get; set; }
        public decimal Price { get; set; }
    }
}
