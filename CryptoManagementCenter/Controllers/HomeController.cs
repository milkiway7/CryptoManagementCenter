using CryptoManagementCenter.Models;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CryptoManagementCenter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IConfiguration configuration)
        {
            _logger = logger;
            _userRepository = userRepository;
            _configuration = configuration;
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
                    UserModel newUser = new UserModel()
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.Now,
                        EmailAddress = user.EmailAddress,
                        Password = _userRepository.HashPassword(user.Password)
                    };

                    await _userRepository.CreateUser(newUser);

                    return View(nameof(Index));
                }
            }

            ModelState.AddModelError("", "Registration failed. Please try agian.");
            return View();
        }
        #endregion

        #region Log in
        public IActionResult Login()
        {
            ViewData["Breadcrumbs"] = new List<BreadcrumbsModel>()
            {
                new BreadcrumbsModel{Name="Log in", Action="Login", Controller="Home" }
            };

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserModel user)
        {
            if(user.Password != null && user.EmailAddress != null)
            {
                UserModel checkIfExsist = await _userRepository.GetUserByEmail(user.EmailAddress);

                if (checkIfExsist != null) { 
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.EmailAddress),
                        new Claim(ClaimTypes.Email, user.EmailAddress)
                    };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, _configuration["CookieName"]);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        IsPersistent = user.RememberMe
                    };

                    await HttpContext.SignInAsync(_configuration["CookieName"],principal, properties);

                    return RedirectToAction(nameof(Index));
                }
            }

            ModelState.AddModelError("", "Email or password are incorrect");
            return View();
        }
        #endregion

        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync(_configuration["CookieName"]);

            return RedirectToAction(nameof(Index));
        }
    }
}
