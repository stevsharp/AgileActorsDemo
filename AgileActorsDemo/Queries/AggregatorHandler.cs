﻿using AgileActorsDemo.Constant;
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

        protected string spotifyUrl = "https://api.spotify.com/v1/search?q=Muse&type=track%2Cartist&market=US&limit=10&offset=5";

        protected string apiUrl => $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

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

            var getWeatherFunc = () => _httpRepository.GetAsync<WeatherApiDto>(apiUrl,cancellationToken);

            Task<WeatherApiDto> getWeatherTask = Task.Run(getWeatherFunc, cancellationToken);

            tasks.Add(getWeatherTask);

            Func<Task< SpotifyDto>> spotifyFunc = () => _spotifyApiHttpRepository.GetAsync<SpotifyDto>(spotifyUrl, cancellationToken);

            Task<SpotifyDto> spotifyTask = Task.Run(spotifyFunc, cancellationToken);

            tasks.Add(spotifyTask);

            var weatherData = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetWeatherCacheKey, getWeatherFunc, DateTimeOffset.Now.AddMinutes(30));

            var spotifyData = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetSpotifyCacheKey, spotifyFunc, DateTimeOffset.Now.AddMinutes(30));

            await Task.WhenAll(tasks);

            return new AggregatorDto(weatherData ?? new WeatherApiDto(), spotifyData ?? new SpotifyDto());
        }
    }
}
