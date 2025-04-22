namespace AFSTranslator.Entities.Common
{
    public static class HttpMethodMapper
    {
        private static readonly Dictionary<ApiType, HttpMethod> HttpMethodMapping = new Dictionary<ApiType, HttpMethod>
        {
            { ApiType.Get, HttpMethod.Get },
            { ApiType.Post, HttpMethod.Post },
            { ApiType.Put, HttpMethod.Put },
            { ApiType.Delete, HttpMethod.Delete },
            { ApiType.Patch, HttpMethod.Patch }
        };

        public static HttpMethod GetHttpMethod(ApiType apiType)
        {
            return HttpMethodMapping.TryGetValue(apiType, out var httpMethod) ? httpMethod : HttpMethod.Get;
        }
    }
}