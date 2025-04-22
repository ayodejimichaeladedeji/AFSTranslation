using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using AFSTranslator.Interfaces.Services;
using AFSTranslator.Entities.Requests;
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

        // [HttpGet]
        // public IActionResult Login()
        // {
        //     LoginRequest model = new();
        //     return View(model);
        // }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequest model = new()
            {
                Username = string.Empty, // or a default value
                Password = string.Empty  // or a default value
            };
            return View(model);
        }

        // [HttpPost]
        // public async Task<IActionResult> Login([FromBody] LoginRequest request)
        // {
        //     var result = await _authService.LoginAsync(request.Username!, request.Password!);

        //     if (!result.IsSuccess)
        //     {
        //         return Unauthorized(result);
        //     }

        //     return Ok(result);
        // }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model.Username, model.Password);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim("JWT", result.Content!.Token!)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Translate", "Translate");
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