using Dapper;
using System.Data;
using AFSTranslator.Models;
using Microsoft.Data.SqlClient;
using AFSTranslator.Interfaces.Repository;

namespace AFSTranslator.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _sqlConnection;

        public UserRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task AddAsync(User user)
        {
            await _sqlConnection.ExecuteAsync("sp_RegisterUser", new { user.Username, user.PasswordHash }, commandType: CommandType.StoredProcedure);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var response = await _sqlConnection.QueryFirstOrDefaultAsync<User>("sp_GetUserByUsername", new { Username = username }, commandType: CommandType.StoredProcedure);
            return response;
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        
        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}