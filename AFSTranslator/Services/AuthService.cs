using Microsoft.AspNetCore.Authentication;

namespace AFSTranslator.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> RegisterAsync(string username, string password)
        {
            Result<string> result = new ();

            try
            {
                var existingUser = await _userRepository.GetByUsernameAsync(username);

                if (existingUser != null)
                {
                    result.ErrorMessage = "Username taken";
                    return result;
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                var user = new User { Username = username, PasswordHash = passwordHash };

                bool addUser = await _userRepository.AddAsync(user);

                if (addUser)
                {
                    result.Content = "Registration successful";
                    result.Message = "Successful request";
                    return result;
                }

                result.ErrorMessage = "Failed to register user";
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"An error occurred while registering the user: {ex.Message}";
                return result;
            }    
        }
        
        public async Task<Result<string>> LoginAsync(string username, string password)
        {
            Result<string> result = new ();

            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);

                if (user == null)
                {
                    result.ErrorMessage = "Invalid username or password";
                    return result;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    result.ErrorMessage = "Invalid username or password";
                    return result;
                }

                result.Content = _tokenService.GenerateToken(user);
                result.Message = "Login successful";

                var claims = new List<Claim>
                {
                    new(ClaimTypes.PrimarySid, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Id.ToString()),
                    new("UserId", result.Content!)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

                await _httpContextAccessor.HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"An error occurred while registering the user: {ex.Message}";
                return result;
            }    
        }
    }
}