using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class NewProjectModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        [Required]
        public string Cryptocurrency { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int InvestmentAmount { get; set; }
        public int? InvestmentFund { get; set; }
        [Required]
        public string InvestmentType { get; set; }
        [Required]
        public string InvestmentStrategy { get; set; }
    }
}
