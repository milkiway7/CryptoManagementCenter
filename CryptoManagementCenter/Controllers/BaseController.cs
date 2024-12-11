using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CryptoManagementCenter.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected UserModel _user;

        public BaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

         public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(User?.Identity?.IsAuthenticated == true)
            {
                _user = await _userRepository.GetUserByEmail(User.Identity.Name);
            }

            await next();
        }

        
    }
}
