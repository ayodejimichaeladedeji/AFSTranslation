
namespace AFSTranslator.Repository
{
    public class TranslationLogRepository : ITranslationLogRepository
    {
        private readonly SqlConnection _sqlConnection;

        public TranslationLogRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<bool> AddAsync(TranslationLog translation)
        {
            try
            {
                return await _sqlConnection.ExecuteAsync("spInsertTranslationLog", new DynamicParameters(translation), commandType: CommandType.StoredProcedure) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public async Task<IEnumerable<TranslationLogResponse>> GetUserTranslationLogsAsync(int userId)
        {
            try
            {
                IEnumerable<TranslationLogResponse> logs = await _sqlConnection.QueryAsync<TranslationLogResponse>("spGetTranslationLogsByUser", new { UserId = userId }, commandType: CommandType.StoredProcedure);
                return logs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
        
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TranslationLog>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TranslationLog> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TranslationLog entity)
        {
            throw new NotImplementedException();
        }
    }
}