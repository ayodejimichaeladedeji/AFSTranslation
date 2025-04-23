using Microsoft.AspNetCore.Authentication;

namespace AFSTranslator.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, IPasswordHasher passwordHasher)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Register(string username, string password)
        {
            Result<string> result = new();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                result.ErrorMessage = "Invalid username or password";
                return result;
            }

            try
            {
                var existingUser = await _userRepository.GetByUsernameAsync(username);

                if (existingUser != null)
                {
                    result.ErrorMessage = "Username taken";
                    return result;
                }

                // var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                var passwordHash = _passwordHasher.Hash(password);

                if (string.IsNullOrWhiteSpace(passwordHash))
                {
                    result.ErrorMessage = "An error occurred";
                    return result;
                }

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

        public async Task<Result<string>> Login(string username, string password)
        {
            Result<string> result = new();

            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);

                if (user == null)
                {
                    result.ErrorMessage = "Invalid username or password";
                    return result;
                }

                bool verifyPassword = _passwordHasher.Verify(user.PasswordHash, password);

                if (!verifyPassword)
                {
                    result.ErrorMessage = "Invalid username or passworddde";
                    return result;
                }

                // result.Content = _tokenService.GenerateToken(user);

                result.Content = "Login successful";
                result.Message = "Successful request";

                var claims = new List<Claim>
                {
                    new(ClaimTypes.PrimarySid, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Id.ToString())
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

        public async Task<Result<string>> Logout()
        {
            Result<string> result = new();

            try
            {
                await _httpContextAccessor.HttpContext.SignOutAsync("MyCookieAuth");
                result.Content = "Logout successful";
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"An error occurred while logging out: {ex.Message}";
                return result;
            }
        }
    }
}