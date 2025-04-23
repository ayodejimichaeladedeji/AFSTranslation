namespace AFSTranslator.Interfaces.Repository
{
    public interface ITranslationLogRepository: IRepository<TranslationLog>
    {
        Task<IEnumerable<TranslationLogResponse>> GetUserTranslationLogsAsync(int userId);
    }
}