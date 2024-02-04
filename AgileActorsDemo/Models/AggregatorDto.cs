using MediatR;

namespace AgileActorsDemo.Models
{
    public record AggregatorDto(WeatherApiDto WeatherDto, SpotifyDto spotifyDto);
    
}
