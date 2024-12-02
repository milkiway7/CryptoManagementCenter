using Microsoft.AspNetCore.Mvc;

namespace CryptoManagementCenter.Controllers
{
    public class NewProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
