using AgileActorsDemo.Constant;
using AgileActorsDemo.Models;
using AgileActorsDemo.Spotify;

using LazyCache;

using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgileActorsDemo.Services
{
    public class SpotifyApiHttpRepository : HttpRepository, ISpotifyApiHttpRepository
    {

        protected string clientId = "5c21c84f5fe542b99286ef2a7eee9db0";
        protected string clientSecret = "aea7bc1b406b46e5832753be0f8d371c";

        protected readonly IAppCache _cache;
        public SpotifyApiHttpRepository(
            IHttpClientFactory httpClientFactory, 
            ILogger<SpotifyApiHttpRepository> logger,
            IAppCache cache)
            : base(httpClientFactory, logger)
        {
            _cache = cache;
        }

        public override async Task<SpotifyDto> GetAsync<SpotifyDto>(string endpoint, CancellationToken cancellationToken = default)
        {

            Func<Task<SpotifyAccessToken>> geSpotifyFunc = async () =>
            {

                string credentials = String.Format("{0}:{1}", clientId, clientSecret);
                using var client = this.CreateHttpClient();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

                List<KeyValuePair<string, string>> requestData = new()
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                };  

                FormUrlEncodedContent requestBody = new(requestData);

                var request = await client.PostAsync("https://accounts.spotify.com/api/token", requestBody).ConfigureAwait(false);

                var response = await request.Content.ReadAsStringAsync();

                var token = JsonConvert.DeserializeObject<SpotifyAccessToken>(response);

                return token;
            };


            var spotifyToken = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetWeatherCacheKey, geSpotifyFunc, DateTimeOffset.Now.AddSeconds(3600));

            var spotifyData = await base.GetAsync<SpotifyDto>(endpoint, spotifyToken.access_token, cancellationToken);

            return spotifyData;

        }

    }
}


//return await _asyncRetryPolicy.ExecuteAsync(async () =>
//{
//    string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

//    var client_ = _httpClientFactory.CreateClient();

//    var data = new WeatherApiDto();

//    try
//    {
//        using var request_ = new HttpRequestMessage();

//        request_.Method = new HttpMethod("GET");
//        request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));


//        request_.RequestUri = new Uri(apiUrl, UriKind.RelativeOrAbsolute);

//        var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

//        var status_ = (int)response_.StatusCode;

//        if (status_ == 200)
//        {

//            var responseText = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

//            data = JsonConvert.DeserializeObject<WeatherApiDto>(responseText);
//        }
//        else if (status_ == 400)
//        {
//            string responseText = response_.Content == null ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
//            throw new Exception($"Problem with the request, such as a missing, invalid or type mismatched parameter. : {responseText}");
//        }
//        else
//        {
//            var responseData = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
//            throw new Exception($"Problem with the request, such as a missing, invalid or type mismatched parameter. :{responseData}");
//        }

//    }
//    catch (Exception ex)
//    {
//        _logger.Log(LogLevel.Error, ex.Message, ex);
//    }


//    return data;
//});
