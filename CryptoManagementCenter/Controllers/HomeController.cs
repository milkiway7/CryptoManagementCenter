using CryptoManagementCenter.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CryptoManagementCenter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            ViewData["Breadcrumbs"] = new List<BreadcrumbsModel>()
            {
                new BreadcrumbsModel{Name="Register",Action="Register",Controller="Home"}
            };

            return View();
        }
    }
}
