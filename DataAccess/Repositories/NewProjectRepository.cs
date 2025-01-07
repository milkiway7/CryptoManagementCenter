using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class NewProjectRepository : BaseRepository, INewProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NewProjectRepository> _logger;
        public NewProjectRepository(ApplicationDbContext context, ILogger<NewProjectRepository> logger) : base(context) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateNewProjectAsync(NewProjectModel model)
        {
            try
            {
                _context.NewProjects.Add(model);
                await SaveAsync();
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex, "DATABASE: New Project creation failed");
                return false;
            }
        }

        public async Task<IEnumerable<NewProjectModel>> GetAllNewProjectsAsync(Guid userId)
        {
            return await _context.NewProjects
                .Where(p => p.CreatedBy == userId)
                .ToListAsync();
        }

        public async Task<bool> UpdateNewProjectAsync(NewProjectModel newProject)
        {
            try
            {
                _context.NewProjects.Update(newProject);
                await SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DATABASE: New Project update failed");
                return false;
            }
        }
    }
}
