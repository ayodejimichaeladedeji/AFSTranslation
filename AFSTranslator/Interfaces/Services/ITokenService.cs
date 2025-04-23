using AFSTranslator.Models;

namespace AFSTranslator.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}