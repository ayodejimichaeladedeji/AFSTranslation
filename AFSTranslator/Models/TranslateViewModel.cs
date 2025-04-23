using AFSTranslator.Entities.Enums;

namespace AFSTranslator.Models
{
    public class TranslateViewModel
    {
        public required List<string> TranslationModes { get; set; }
        [Required]
        public string SelectedMode { get; set; }
        [Required(ErrorMessage = "The text to translate field is required.")]
        [StringLength(500, ErrorMessage = "Text to translate cannot exceed 5000 characters.")]
        public string TextToTranslate { get; set; }
        public string TranslatedText { get; set; }
    }
}