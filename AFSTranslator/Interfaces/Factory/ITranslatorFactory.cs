using AFSTranslator.Interfaces.Services;

namespace AFSTranslator.Interfaces.Factory
{
    public interface ITranslatorFactory
    {
         ITranslatorService GetTranslatorService();
    }
}