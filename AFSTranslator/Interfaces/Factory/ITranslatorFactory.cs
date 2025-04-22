namespace AFSTranslator.Interfaces.Factory
{
    public interface ITranslatorFactory
    {
         ITranslatorService GetTranslatorService();
    }
}