using Newtonsoft.Json;

using Polly;
using Polly.Retry;

using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AgileActorsDemo.Services
{

    public class HttpRepository : IHttpRepository
    {
        protected readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<HttpRepository> _logger;

        protected readonly JsonSerializerOptions _options;

        protected const int MaxRetries = 3;

        protected readonly AsyncRetryPolicy _asyncRetryPolicy;

        public HttpRepository(IHttpClientFactory httpClientFactory, ILogger<HttpRepository> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            _asyncRetryPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(MaxRetries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        protected HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient();

            return client;
        }

        public  virtual async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default) where T : class
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException($"'{nameof(endpoint)}' cannot be null or empty.", nameof(endpoint));

            return await _asyncRetryPolicy.ExecuteAsync(async () =>
            {
                using var response = await CreateHttpClient().GetAsync(endpoint,
                        HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);

                var statusCode = response.StatusCode;

                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var returnList = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream, _options, cancellationToken);

                return returnList;
            }
            );
        }

        public async Task<T?> GetAsync<T>(string endpoint, string token, CancellationToken cancellationToken = default) where T : class
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException($"'{nameof(endpoint)}' cannot be null or empty.", nameof(endpoint));

            return await _asyncRetryPolicy.ExecuteAsync(async () =>
            {
                var client = this.CreateHttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                using var response = await client.GetAsync(endpoint,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken)
                    .ConfigureAwait(false);

                var statusCode = response.StatusCode;

                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var returnList = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream, _options, cancellationToken);

                return returnList;
            }
            );
        }

        public async Task<T?> GetAsync<T>(Uri endpoint, string token, CancellationToken cancellationToken = default) where T : class
        {
            var client = CreateHttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return await _asyncRetryPolicy.ExecuteAsync(async () =>
            {
                using var response = await client.GetAsync(endpoint,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken)
                    .ConfigureAwait(false);

                try
                {
                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                    var returnList = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream, _options, cancellationToken);

                    return returnList;
                }
                catch (Exception e)
                {
                    var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);

                    var responseData = JsonConvert.DeserializeObject<T>(jsonString);

                    return responseData;
                }
            }
            );
        }

        public async Task<T?> GetAsyncWithJsonConvert<T>(Uri endpoint, string token, CancellationToken cancellationToken = default) where T : class
        {
            var client = CreateHttpClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            return await _asyncRetryPolicy.ExecuteAsync(async () =>
            {
                using var response = await client.GetAsync(endpoint,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken)
                    .ConfigureAwait(false);

                try
                {
                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStringAsync(cancellationToken);

                    var returnList = JsonConvert.DeserializeObject<T>(stream);

                    return returnList;
                }
                catch (Exception e)
                {
                    var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);

                    var responseData = JsonConvert.DeserializeObject<T>(jsonString);

                    return responseData;
                }
            }
            );
        }

        public async Task<T?> PostAsync<T>(object data, Uri endpoint, CancellationToken cancellationToken = default) where T : class
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);

            return await _asyncRetryPolicy.ExecuteAsync(async () =>
            {
                var response = await client.PostAsync(endpoint, new JsonContent(data), cancellationToken);

                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                var dto = System.Text.Json.JsonSerializer.Deserialize<T>(content, _options);

                return dto;

            });
        }

    }
}

