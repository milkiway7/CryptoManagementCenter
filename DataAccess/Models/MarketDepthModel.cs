using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class MarketDepthModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Symbol { get; set; }
        [Required]
        public long TimeStampMs { get; set; }
        [Required]
        public string? Bids { get; set; }
        [Required]
        public string? Asks { get; set; }

        [NotMapped]
        public List<List<string>> BidList
        {
            get => JsonSerializer.Deserialize<List<List<string>>>(Bids) ?? new();
            set => Bids = JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public List<List<string>> AskList
        {
            get => JsonSerializer.Deserialize<List<List<string>>>(Asks) ?? new();
            set => Asks = JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public DateTime GetDateTime
        {
            get => DateTimeOffset.FromUnixTimeMilliseconds(TimeStampMs).LocalDateTime;
            set => TimeStampMs = new DateTimeOffset(value).ToUnixTimeMilliseconds();
        }
    }
}
