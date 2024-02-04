using AgileActorsDemo.Constant;
using AgileActorsDemo.Models;
using AgileActorsDemo.Services;

using LazyCache;

using MediatR;

namespace AgileActorsDemo.Queries
{
    internal class AggregatorHandler : IRequestHandler<AggregatorQuery, AggregatorDto>
    {

        protected readonly IHttpRepository _httpRepository;

        protected readonly IAppCache _cache;

        protected readonly ISpotifyApiHttpRepository _spotifyApiHttpRepository;

        protected string apiKey = "d3d9fd2b66ba1fbb58696115bd6a424f";

        protected string city = "London"; // Replace with the desired city name

        public AggregatorHandler(
            IHttpRepository weatherApiHttpRepository, 
            IAppCache cache, 
            ISpotifyApiHttpRepository spotifyApiHttpRepository)
        {
            _httpRepository = weatherApiHttpRepository;

            _cache = cache;
            _spotifyApiHttpRepository = spotifyApiHttpRepository;
        }
        public async Task<AggregatorDto> Handle(AggregatorQuery request, CancellationToken cancellationToken)
        {
            List<Task> tasks = new();

            string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

            var getWeatherFunc = () => _httpRepository.GetAsync<WeatherApiDto>(apiUrl,cancellationToken);

            Task<WeatherApiDto> getWeatherTask = Task.Run(getWeatherFunc, cancellationToken);

            tasks.Add(getWeatherTask);

            var weatherData = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetWeatherCacheKey, getWeatherFunc, DateTimeOffset.Now.AddMinutes(30));

            await Task.WhenAll(tasks);

            return new AggregatorDto(weatherData ?? new WeatherApiDto());
        }
    }
}
