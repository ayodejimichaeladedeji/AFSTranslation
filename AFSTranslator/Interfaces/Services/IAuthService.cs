using AFSTranslator.Models;
using AFSTranslator.Entities;
using AFSTranslator.Entities.Responses;

namespace AFSTranslator.Interfaces.Services
{
    public interface IAuthService
    {
        // Task SignOutAsync();
        // Task SignInAsync(User user);
        Task<Result<string>> RegisterAsync(string username, string password);
        Task<Result<LoginResponse>> LoginAsync(string username, string password);
    }
}