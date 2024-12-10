using DataAccess.Constants;
using DataAccess.Helpers;
using DataAccess.Models;
using DataAccess.Models.DTO;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoManagementCenter.Controllers
{
    [Authorize]
    public class NewProjectController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly INewProjectRepository _newProjectRepository;
        public NewProjectController(IUserRepository userRepository, INewProjectRepository newProjectRepository)
        {
            _userRepository = userRepository;
            _newProjectRepository = newProjectRepository;
        }

        public IActionResult Index()
        {
            ViewData["Breadcrumbs"] = new List<BreadcrumbsModel>()
            {
                new BreadcrumbsModel(){Name="New project"}
            };
            return View();
        }

        [HttpPost]
        [Route("NewProject/AddProjectAsync")]
        public async Task<IActionResult> CreateNewProjectAsync([FromBody] NewProjectDto data )
        {
            if (!ModelState.IsValid) BadRequest(new { error = true, message = "Error: no data provided" });

            UserModel user = await _userRepository.GetUserByEmail(User.Identity.Name);

            if(user == null) StatusCode(500, new { error = true, message = "Error: user not found" });

            NewProjectModel newProject = DtoMapper.MapNewProject(data, user.Id, NewProjectConstants.Statuses.Created);

            if (!TryValidateModel(newProject)) StatusCode(500, new { error = true, message = "Error: New Project Model is invalid" });

            bool success = await _newProjectRepository.CreateNewProjectAsync(newProject);

            if (success)
            {
                return Ok(new { success = true, message = "New project created", id = newProject.Id, status = newProject.Status, createdAt = newProject.CreatedAt, createdBy = user.EmailAddress });
            }
            else
            {
                return StatusCode(500, new { error = true, message = "Server error: new project creation failed" });
            }
        }
    }
}
