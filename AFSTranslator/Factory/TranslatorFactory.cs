using AFSTranslator.Interfaces.Factory;
using AFSTranslator.Interfaces.Services;

namespace AFSTranslator.Factory
{
    public class TranslatorFactory: ITranslatorFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IEnumerable<ITranslatorService> _translators;

        public TranslatorFactory(IEnumerable<ITranslatorService> translators, IConfiguration configuration)
        {
            _configuration = configuration;
            _translators = translators;
        }

        public ITranslatorService GetTranslatorService()
        {
            string defaultTranslationMode = _configuration.GetValue<string>("TranslationSettings:DefaultMode")!;

            var translator = _translators.FirstOrDefault(t => t.Name.Equals(defaultTranslationMode, StringComparison.OrdinalIgnoreCase));

            if (translator == null)
            {
                throw new ArgumentException($"Translator with name {defaultTranslationMode} not found.");
            }
            
            return translator;  
        }
    }
}