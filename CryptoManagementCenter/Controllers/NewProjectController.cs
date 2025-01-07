using DataAccess.Constants;
using DataAccess.Helpers;
using DataAccess.Models;
using DataAccess.Models.DTO;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CryptoManagementCenter.Controllers
{
    [Authorize]
    public class NewProjectController : BaseController
    {
        private readonly INewProjectRepository _newProjectRepository;

        public NewProjectController(IUserRepository userRepository, INewProjectRepository newProjectRepository) : base(userRepository)
        {
            _newProjectRepository = newProjectRepository;
        }

        public IActionResult Index()
        {
            var createdProjectJson = TempData["CreatedProject"];

            ViewData["Breadcrumbs"] = new List<BreadcrumbsModel>()
            {
                new BreadcrumbsModel(){Name="New project"}
            };

            ViewData["ProjectJson"] = createdProjectJson;

            return View();
        }

        [HttpPost]
        [Route("NewProject/AddProjectAsync")]
        public async Task<IActionResult> CreateNewProjectAsync([FromBody] NewProjectDto data)
        {
            if (!ModelState.IsValid) BadRequest(new { error = true, message = "Error: no data provided" });

            if (_user == null) StatusCode(500, new { error = true, message = "Error: user not found" });

            NewProjectModel newProject = DtoMapper.MapNewProject(data, _user.Id, NewProjectConstants.Statuses.Created);

            if (!TryValidateModel(newProject)) StatusCode(500, new { error = true, message = "Error: New Project Model is invalid" });

            bool success = await _newProjectRepository.CreateNewProjectAsync(newProject);

            if (success)
            {
                return Ok(new { success = true, message = "New project created", id = newProject.Id, status = newProject.Status, createdAt = newProject.CreatedAt, createdBy = _user.EmailAddress });
            }
            else
            {
                return StatusCode(500, new { error = true, message = "Server error: new project creation failed" });
            }
        }

        [HttpPatch]
        [Route("NewProject/ProcessForm")]
        public async Task<IActionResult> ProcessFormAsync([FromBody] NewProjectDto data)
        {
            if (!ModelState.IsValid) BadRequest(new { error = true, message = "Error: no data provided" });

            if (_user == null) StatusCode(500, new { error = true, message = "Error: user not found" });

            NewProjectModel project = DtoMapper.MapNewProject(data, _user.Id, data.Status);

            bool success = await _newProjectRepository.UpdateNewProjectAsync(project);

            if (success)
            {
                return Ok(new { success = true, message = "New project updated" });
            }
            else
            {
                return StatusCode(500, new { error = true, message = "Server error: new project update failed" });
            }
        }

        #region New Project Report
        public IActionResult Report()
        {
            ViewData["Breadcrumbs"] = new List<BreadcrumbsModel>()
            {
                new BreadcrumbsModel(){Name="New project report"}
            };
            return View();
        }

        [HttpGet]
        [Route("NewProject/ProjectReport")]
        public async Task<IActionResult> GetReportAsync()
        {
            IEnumerable<NewProjectModel> projects = await _newProjectRepository.GetAllNewProjectsAsync(_user.Id);

            List<NewProjectDto> projectsDto = new List<NewProjectDto>();

            foreach(NewProjectModel project in projects)
            {
                projectsDto.Add(DtoMapper.MapToNewProjectDto(project));
            }

            foreach (NewProjectDto dto in projectsDto)
            {
                dto.CreatedBy = _user.EmailAddress;
            }

            return Json(projectsDto);
        }

        [HttpPost]
        [Route("NewProject/Edit")]
        public IActionResult EditProject([FromBody] NewProjectDto project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = true, message = "Error: no data provided" });
            }

            if (_user == null)
            {
                return StatusCode(500, new { error = true, message = "Error: user not found" });
            }

            // Zapisz model w TempData
            TempData["CreatedProject"] = JsonConvert.SerializeObject(project);

            // Zwróć odpowiedź JSON
            return Ok(new { redirectUrl = Url.Action("Index") });
        }
        #endregion
    }
}
