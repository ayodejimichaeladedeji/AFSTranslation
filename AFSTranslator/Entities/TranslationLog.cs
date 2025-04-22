namespace AFSTranslator.Entities
{
    public class TranslationLog
    {
        public int UserId { get; set; }
        public string Mode { get; set; }
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
    }
}