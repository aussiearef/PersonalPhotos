using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPhotos.Models;

namespace PersonalPhotos.Controllers
{
    public class LoginsController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogins _loginService;

        public LoginsController(ILogins loginService, IHttpContextAccessor httpContextAccessor)
        {
            _loginService = loginService;
            _httpContextAccessor = httpContextAccessor;

        }

        public IActionResult Index(string returnUrl = null)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl};
            return View("Login", model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid login detils");
                return View("Login", model);
            }

            var user = await _loginService.GetUser(model.Email);
            if (user != null)
            {
                if (user.Password == model.Password)
                {
                    //ToDo: redirect to home page
                    _httpContextAccessor.HttpContext.Session.SetString("User", model.Email);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid password");
                    return View("Login", model);
                }
            }
            else
            {
                ModelState.AddModelError("", "User was not found");
                return View("Login", model);
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return RedirectToAction("Display", "Photos");
            }
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid user details");
                return View(model);
            }

            var existingUser = await _loginService.GetUser(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "This email adress is already registered");
                return View(model);
            }

            await _loginService.CreateUser(model.Email, model.Password);

            return RedirectToAction("Index", "Logins");
        }
    }
}