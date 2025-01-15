using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class RecentTradeModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("price")]
        public string? Price { get; set; }
        [JsonPropertyName("qty")]
        public string? Qty { get; set; }
        [JsonPropertyName("quoteQty")]
        public string? QuoteQty { get; set; }
        [JsonPropertyName("time")]
        public long Time { get; set; }
        public DateTime? TradeDate { get; set; }
    }
}
