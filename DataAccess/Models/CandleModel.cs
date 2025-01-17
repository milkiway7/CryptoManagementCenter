using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class CandleModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Symbol { get; set; } = "BTCUSDT";
        [Required]
        public long OpenTime { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Open { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal High { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Low { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Close { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Volume { get; set; }
    }
}
