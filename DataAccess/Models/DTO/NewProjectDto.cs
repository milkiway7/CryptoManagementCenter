using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO
{
    public class NewProjectDto
    {
        //[JsonPropertyName("id")]
        //public int? Id { get; set; }
        //[JsonPropertyName("createdAt")]
        //public DateTime? CreatedAt { get; set; }
        //[JsonPropertyName("createdBy")]
        //public string? CreatedBy { get; set; }
        //[JsonPropertyName("status")]
        //public string? Status { get; set; }
        [Required]
        [JsonPropertyName("projectName")]
        public string ProjectName { get; set; }
        [JsonPropertyName("projectDescription")]
        public string? ProjectDescription { get; set; }
        [Required]
        [JsonPropertyName("cryptocurrency")]
        public string Cryptocurrency { get; set; }
        [Required]
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        [Required]
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
        [Required]
        [JsonPropertyName("investmentAmount")]
        public int InvestmentAmount { get; set; }
        [JsonPropertyName("investmentFund")]
        public int? InvestmentFund { get; set; }
        [Required]
        [JsonPropertyName("investmentType")]
        public string InvestmentType { get; set; }
        [Required]
        [JsonPropertyName("investmentStrategy")]
        public string InvestmentStrategy { get; set; }
    }
}
