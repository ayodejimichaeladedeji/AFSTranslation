


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
                throw new Exception("An error occurred while logging the translation", ex);
            }
        }

        public async Task<IEnumerable<TranslationLog>> GetUserTranslationLogs(int userId)
        {
            try
            {
                return await _translationLogRepository.GetUserTranslationLogsAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the translation logs by user", ex);
            }
        }
    }
}