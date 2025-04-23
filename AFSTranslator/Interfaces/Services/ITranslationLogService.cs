namespace AFSTranslator.Interfaces.Services
{
    public interface ITranslationLogService
    {
        Task<bool> LogTranslation(TranslationLog translationLog);
        Task<IEnumerable<TranslationLogResponse>> GetUserTranslationLogs(int userId);
    }
}