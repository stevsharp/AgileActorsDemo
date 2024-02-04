using MediatR;

namespace AgileActorsDemo.Models
{
    public record AggregatorDto
    {
        public WeatherApiDto WeatherDto { get; }

        public AggregatorDto(WeatherApiDto weatherDto) => WeatherDto = weatherDto;

    }
}
