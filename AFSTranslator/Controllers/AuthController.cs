using Microsoft.AspNetCore.Mvc;
using AFSTranslator.Interfaces.Services;
using AFSTranslator.Entities.Requests;

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
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Username!, request.Password!);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public IActionResult Registration() => View();

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationRequest request)
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