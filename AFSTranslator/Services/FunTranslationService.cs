using AFSTranslator.Entities;
using AFSTranslator.Entities.Enums;
using AFSTranslator.Entities.Common;
using AFSTranslator.Entities.Responses;
using AFSTranslator.Interfaces.Services;

namespace AFSTranslator.Services
{
    public class FunTranslationService : ITranslatorService
    {
        public string Name => "FunTranslation";
        private readonly IRestService _restService;
        private readonly IConfiguration _configuration;
        public List<string> Modes => FunTranslationModes.GetNames(typeof(FunTranslationModes)).ToList();


        public FunTranslationService(IRestService restService, IConfiguration configuration)
        {
            _restService = restService;
            _configuration = configuration;
        }

        public async Task<Result<string>> Translate(string mode, string textToTranslate)
        {
            Result<string> result = new();

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