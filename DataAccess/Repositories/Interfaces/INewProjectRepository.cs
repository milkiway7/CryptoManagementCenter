using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface INewProjectRepository
    {
        Task<bool> CreateNewProjectAsync(NewProjectModel model);
        Task<bool> UpdateNewProjectAsync(NewProjectModel newProject);
    }
}
