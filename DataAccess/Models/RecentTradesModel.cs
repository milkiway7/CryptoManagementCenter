using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class RecentTradesModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public long TimeStampMs { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Price { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Quantity { get; set; }
        [Required]
        public bool IsBuyerMaker { get; set; }

    }
}
