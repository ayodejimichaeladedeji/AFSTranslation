namespace AFSTranslator.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<string>> Register(string username, string password);
        Task<Result<string>> Login(string username, string password);
        Task<Result<string>> Logout();
    }
}