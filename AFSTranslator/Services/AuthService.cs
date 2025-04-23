using AFSTranslator.Models;
using AFSTranslator.Entities;
using AFSTranslator.Entities.Responses;
using AFSTranslator.Interfaces.Services;
using AFSTranslator.Interfaces.Repository;

namespace AFSTranslator.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<Result<string>> RegisterAsync(string username, string password)
        {
            Result<string> result = new ();

            var existingUser = await _userRepository.GetByUsernameAsync(username);

            if (existingUser != null)
            {
                result.ErrorMessage = "Username taken";
                return result;
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User { Username = username, PasswordHash = passwordHash };

            await _userRepository.AddAsync(user);

            result.Content = "Registration successful";
            result.Message = "Successful request";

            return result;
        }
        
        public async Task<Result<LoginResponse>> LoginAsync(string username, string password)
        {
            Result<LoginResponse> result = new ();

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

            LoginResponse loginResponse = new() { Token = _tokenService.GenerateToken(user) };
            result.Content = loginResponse;
            result.Message = "Login successful";    

            return result;
        }
    }
}