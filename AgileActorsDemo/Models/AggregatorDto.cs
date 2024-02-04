using MediatR;

namespace AgileActorsDemo.Models
{
    public record AggregatorDto(WeatherApiDto weatherDto, SpotifyDto spotifyDto, NewsApiDto newsApiDto);
    
}
