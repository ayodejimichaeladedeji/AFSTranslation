namespace AFSTranslator.Interfaces.Services
{
    public interface ITranslatorService
    {
        Task<Result<string>> Translate(string mode, string textToTranslate);
        string Name { get; }
        List<string> Modes { get;}
    }
}