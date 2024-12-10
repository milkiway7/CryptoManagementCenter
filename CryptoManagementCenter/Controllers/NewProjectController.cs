using DataAccess.Constants;
using DataAccess.DTO;
using DataAccess.Helpers;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoManagementCenter.Controllers
{
    [Authorize]
    public class NewProjectController : Controller
    {
        private readonly IUserRepository _userRepository;
        public NewProjectController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            if(user == null) BadRequest(new { error = true, message = "Error: user not found" });

            NewProjectModel newProject = DtoMapper.MapNewProject(data, user.Id, NewProjectConstants.Statuses.Created);



            return View();
        }
    }
}
