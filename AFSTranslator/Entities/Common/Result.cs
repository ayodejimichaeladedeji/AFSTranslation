namespace AFSTranslator.Entities
{
    public class Result<T>
    {
        public T? Content {get; set;}
        public string Message { get; set; } = "";
        public bool HasError => ErrorMessage != "";
        public string ErrorMessage { get; set; } = "";
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }
}