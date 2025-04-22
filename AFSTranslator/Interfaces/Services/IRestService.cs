using AFSTranslator.Entities;
using AFSTranslator.Entities.Common;

namespace AFSTranslator.Interfaces.Services
{
    public interface IRestService
    {
        Task<Result<T>> MakeRequest<T>(string url, ApiType apiType, object payload, Dictionary<string, string> headers = null);
    }
}