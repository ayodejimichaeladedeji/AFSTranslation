using AFSTranslator.Entities;
using AFSTranslator.Entities.Common;
using AFSTranslator.Interfaces.Services;
using Newtonsoft.Json;

namespace AFSTranslator.Services
{
    public class RestService : IRestService
    {
        private readonly IHttpClientFactory _client;
        public RestService(IHttpClientFactory client)
        {
            _client = client;
        }

        public async Task<Result<T>> MakeRequest<T>(string url, ApiType apiType, object payload, Dictionary<string, string> headers = null)
        {
            var result = new Result<T>();
            var requestResponse = new HttpResponseMessage();

            try
            {
                var client = _client.CreateClient();
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(url);
                request.Method = HttpMethodMapper.GetHttpMethod(apiType);

                if (payload is not null && apiType != ApiType.Get)
                {
                    if (headers?.ContainsKey("Content-Type") == true && headers["Content-Type"] == "application/x-www-form-urlencoded")
                    {
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(payload));
                        request.Content = new FormUrlEncodedContent(dict!);
                    }
                    else
                    {
                        string serializedPayload = JsonConvert.SerializeObject(payload);
                        request.Content = new StringContent(serializedPayload, System.Text.Encoding.UTF8, "application/json");
                    }
                }
                
                if (headers is not null)
                {
                    foreach (var header in headers)
                    {
                        if (header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                            continue;
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                requestResponse = await client.SendAsync(request);

                string responseBody = await requestResponse.Content.ReadAsStringAsync();

                if (requestResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        result.Message = "Request was successful";
                        result.Content = JsonConvert.DeserializeObject<T>(responseBody);
                    }
                    catch
                    {
                        result.ErrorMessage = "Invalid response format";
                    }
                }
                else
                {
                    result.Content = JsonConvert.DeserializeObject<T>(responseBody);
                    result.ErrorMessage = $"Code: {requestResponse.StatusCode}. An error occurred while processing the request";
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