using Microsoft.AspNetCore.Mvc;

namespace CryptoManagementCenter.Controllers
{
    public class ChartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
