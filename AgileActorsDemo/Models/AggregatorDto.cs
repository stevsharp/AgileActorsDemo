using MediatR;

namespace AgileActorsDemo.Models
{
    public record AggregatorDto
    {
        public WeatherApiDto WeatherDto { get; }

        public AggregatorDto(WeatherApiDto weatherDto)
        {
            WeatherDto = weatherDto;
        }

    }


    public class AggregatorQuery : IRequest<AggregatorDto>
    {
        public string DataSource { get; set; }
    }

}
