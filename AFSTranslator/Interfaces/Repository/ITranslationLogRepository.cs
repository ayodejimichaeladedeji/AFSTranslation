namespace AFSTranslator.Interfaces.Repository
{
    public interface ITranslationLogRepository: IRepository<TranslationLog>
    {
        Task<IEnumerable<TranslationLog>> GetUserTranslationLogsAsync(int userId);
    }
}