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
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [Required]
        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [Required]
        [JsonPropertyName("createdBy")]
        public string? CreatedBy { get; set; }
        [Required]
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [Required]
        [JsonPropertyName("projectName")]
        public string? ProjectName { get; set; }
        [JsonPropertyName("projectDescription")]
        public string? ProjectDescription { get; set; }
        [Required]
        [JsonPropertyName("cryptocurrency")]
        public string? Cryptocurrency { get; set; }
        [Required]
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }
        [Required]
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }
        [Required]
        [JsonPropertyName("investmentAmount")]
        public int? InvestmentAmount { get; set; }
        [JsonPropertyName("investmentFund")]
        public int? InvestmentFund { get; set; }
        [JsonPropertyName("investmentType")]
        public string? InvestmentType { get; set; }
        [JsonPropertyName("investmentStrategy")]
        public string? InvestmentStrategy { get; set; }
    }
}
