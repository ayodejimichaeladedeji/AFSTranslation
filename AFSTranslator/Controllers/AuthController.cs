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

            var result = await _authService.LoginAsync(model.Username, model.Password);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }
            
            return RedirectToAction("Translate", "Translate");
        }

        [HttpGet]
        public IActionResult Registration() => View();

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationRequestViewModel request)
        {
            var result = await _authService.RegisterAsync(request.Username, request.Password);

            if (!result.IsSuccess)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View(request);
            }

            return RedirectToAction("Login");
        }
    }
}