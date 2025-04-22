namespace AFSTranslator.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _sqlConnection;

        public UserRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<bool> AddAsync(User user)
        {
            try
            {
                return await _sqlConnection.ExecuteAsync("sp_RegisterUser", new { user.Username, user.PasswordHash }, commandType: CommandType.StoredProcedure) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user", ex);
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {  
                var response = await _sqlConnection.QueryFirstOrDefaultAsync<User>("sp_GetUserByUsername", new { Username = username }, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the user", ex);
            }
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