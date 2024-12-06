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
    }
}
