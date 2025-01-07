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

        public static NewProjectDto MapToNewProjectDto(NewProjectModel newProject)
        {
            return (new NewProjectDto
            {
                Id = newProject.Id,
                CreatedAt = newProject.CreatedAt,
                CreatedBy = newProject.CreatedBy.ToString(),
                Status = newProject.Status,
                ProjectName = newProject.ProjectName,
                ProjectDescription = newProject.ProjectDescription,
                Cryptocurrency = newProject.Cryptocurrency,
                StartDate = newProject.StartDate,
                EndDate = newProject.EndDate,
                InvestmentAmount = newProject.InvestmentAmount,
                InvestmentFund = newProject.InvestmentFund,
                InvestmentType= newProject.InvestmentType,
                InvestmentStrategy= newProject.InvestmentStrategy,
            });
        }
    }
}
