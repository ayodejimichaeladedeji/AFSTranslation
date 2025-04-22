using AFSTranslator.Entities.Enums;

namespace AFSTranslator.Models
{
    public class TranslateViewModel
    {
        public required List<string> TranslationModes { get; set; }
        public string SelectedMode { get; set; }
        public string TextToTranslate { get; set; }
        public string TranslatedText { get; set; }
    }
}