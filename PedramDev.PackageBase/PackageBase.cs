using System.Text.Json;
using System.Text;

namespace PedramDev.PackageBases
{
    public class PackageBase(IHttpClientFactory factory)
    {
        private readonly IHttpClientFactory _factory = factory ?? throw new Exception("HttpClientFactory is null => builder.Services.AddHttpClient()");

        public async Task<ModelType> Get<ModelType>(string path, string baseUrl)
            where ModelType : class, new()
        {
            using var client = _factory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);

            var httpGetResponse = await client.GetAsync(path);
            var data = await httpGetResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ModelType>(data);
        }
        public async Task<string> Get(string path, string baseUrl)
        {
            using var client = _factory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);

            var httpGetResponse = await client.GetAsync(path);
            return await httpGetResponse.Content.ReadAsStringAsync();
        }

        public async Task Post(string path, string baseUrl)
        {
            using var client = _factory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);

            _ = await client.PostAsync(path, null);
        }
        public async Task Post<ModelType>(string path, string baseUrl, ModelType request)
        {
            using var client = _factory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);

            var body = JsonSerializer.Serialize(request);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            _ = await client.PostAsync(path, content);
        }
    }

}
