using DataAccess.Models;
using DataAccess.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    public static class DtoMapper
    {
        public static NewProjectModel MapNewProject(NewProjectDto dto, Guid createdBy, string status)
        {
            return (new NewProjectModel
            {
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                CreatedBy = createdBy,
                Status = status,
                ProjectName = dto.ProjectName,
                ProjectDescription = dto.ProjectDescription,
                Cryptocurrency = dto.Cryptocurrency,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                InvestmentAmount = dto.InvestmentAmount,
                InvestmentFund = dto.InvestmentFund,
                InvestmentType = dto.InvestmentType,
                InvestmentStrategy = dto.InvestmentStrategy,
            });
        }
    }
}
