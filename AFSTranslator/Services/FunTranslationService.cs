namespace AFSTranslator.Services
{
    public class FunTranslationService : ITranslatorService
    {
        public string Name => "FunTranslation";
        private readonly IRestService _restService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITranslationLogService _translationLogService;
        
        public List<string> Modes => FunTranslationModes.GetNames(typeof(FunTranslationModes)).ToList();


        public FunTranslationService(IRestService restService, IConfiguration configuration, ITranslationLogService translationLogService, IHttpContextAccessor httpContextAccessor)
        {
            _restService = restService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _translationLogService = translationLogService;
        }

        public async Task<Result<string>> Translate(string mode, string textToTranslate)
        {
            Result<string> result = new();

            if (string.IsNullOrWhiteSpace(textToTranslate))
            {
                result.ErrorMessage = "Text to translate cannot be empty";
                return result;
            }

            if (string.IsNullOrWhiteSpace(mode) || !Modes.Contains(mode, StringComparer.OrdinalIgnoreCase))
            {
                result.ErrorMessage = "Invalid translation mode";
                return result;
            }


            try
            {
                Result<FunTranslationResponse> apiResult = new();

                string url = _configuration.GetValue<string>("TranslationSettings:FunTranslation:BaseUrl")!.Replace("*", mode);

                Dictionary<string, string> headers = new();
                headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var payload = new { text = textToTranslate };

                apiResult = await _restService.MakeRequest<FunTranslationResponse>(url, ApiType.Post, payload, headers);

                if (apiResult.IsSuccess)
                {
                    result.Content = apiResult.Content?.Contents?.Translated;
                    TranslationLog translationLog = new()
                    {
                        UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value),
                        Mode = mode,
                        OriginalText = textToTranslate,
                        TranslatedText = result.Content,
                    };
                    await _translationLogService.LogTranslation(translationLog);
                }
                else
                {
                    result.ErrorMessage = apiResult.ErrorMessage;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                return result;
            }
        }
    }
}