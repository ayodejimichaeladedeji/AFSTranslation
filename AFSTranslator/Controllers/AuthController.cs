using AFSTranslator.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using AFSTranslator.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;

namespace AFSTranslator.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.Login(model.Username, model.Password);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            RegistrationRequestViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.Register(model.Username, model.Password);

            if (!result.IsSuccess)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View(model);
            }

            return RedirectToAction("Login");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Login", "Auth");
        }
    }
}