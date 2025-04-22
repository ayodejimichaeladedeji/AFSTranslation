namespace AFSTranslator.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<string>> RegisterAsync(string username, string password);
        Task<Result<string>> LoginAsync(string username, string password);
    }
}