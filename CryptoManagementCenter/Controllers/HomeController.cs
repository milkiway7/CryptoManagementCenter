using CryptoManagementCenter.Models;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CryptoManagementCenter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
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

        #region Register
        public IActionResult Register()
        {
            ViewData["Breadcrumbs"] = new List<BreadcrumbsModel>()
            {
                new BreadcrumbsModel{Name="Register",Action="Register",Controller="Home"}
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(UserModel user)
        {
            if(ModelState.IsValid)
            {
                UserModel checkIfExsist = await _userRepository.GetUserByEmail(user.EmailAddress);

                if(checkIfExsist == null)
                {
                    await _userRepository.CreateUser(user);

                    return View(nameof(Index));
                }
            }

            ModelState.AddModelError("", "Registration failed. Please try agian.");
            return View();
        }
        #endregion
    }
}
