


namespace AFSTranslator.Services
{
    public class TranslationLogService : ITranslationLogService
    {
        private readonly ITranslationLogRepository _translationLogRepository;
        public TranslationLogService(ITranslationLogRepository translationLogRepository)
        {
            _translationLogRepository = translationLogRepository;
        }

        public async Task<bool> LogTranslation(TranslationLog translationLog)
        {
            try
            {
                return await _translationLogRepository.AddAsync(translationLog);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public async Task<IEnumerable<TranslationLogResponse>> GetUserTranslationLogs(int userId)
        {
            try
            {
                return await _translationLogRepository.GetUserTranslationLogsAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}