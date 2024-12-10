using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoManagementCenter.Controllers
{
    [Authorize]
    public class NewProjectController : Controller
    {
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
        public async Task<IActionResult> CreateNewProjectAsync([FromBody] NewProjectModel data )
        {
            if (data == null) BadRequest(new { error = true, message = "Error: no data provided" });

            return View();
        }
    }
}
