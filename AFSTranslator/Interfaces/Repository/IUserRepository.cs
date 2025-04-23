using AFSTranslator.Models;

namespace AFSTranslator.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}