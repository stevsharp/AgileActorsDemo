using AgileActorsDemo.Models;

using Polly;
using Polly.Retry;

namespace AgileActorsDemo.Services
{

    public class WeatherApiHttpRepository : HttpRepository, IWeatherApiHttpRepository
    {
        protected string apiKey = "d3d9fd2b66ba1fbb58696115bd6a424f";

        protected string city = "London"; // Replace with the desired city name

        protected readonly ILogger<WeatherApiHttpRepository> _logger;

        public WeatherApiHttpRepository(IHttpClientFactory httpClientFactory, ILogger<WeatherApiHttpRepository> logger)
            :base(httpClientFactory,logger)
        {

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
