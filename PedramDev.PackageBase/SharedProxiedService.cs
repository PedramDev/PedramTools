using System.Net;

namespace PedramDev.PackageBases
{
    public abstract class SharedProxiedService : IDisposable
    {
        protected HttpClientHandler _httpClientHandler;
        protected HttpClient _httpClient;

        public void ClientFactory(string proxyUrl)
        {
            if (!string.IsNullOrEmpty(proxyUrl))
            {

                var finalProxy = $"http://{proxyUrl}";
                var proxy = new WebProxy(finalProxy)
                {
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                };

                _httpClientHandler = new HttpClientHandler
                {
                    UseProxy = true,
                    Proxy = proxy,

                    // Disable SSL verification
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                _httpClient = new HttpClient(_httpClientHandler)
                {
                    Timeout = TimeSpan.FromSeconds(2000)
                };
            }
            else
            {
                _httpClient = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(2000)
                };
            }

            _httpClient.DefaultRequestHeaders.Clear();
        }

        public void Dispose()
        {
            _httpClientHandler?.Dispose();
            _httpClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
