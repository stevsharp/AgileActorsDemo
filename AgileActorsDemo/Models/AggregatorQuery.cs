using MediatR;

namespace AgileActorsDemo.Models
{
    public class AggregatorQuery : IRequest<AggregatorDto>
    {
        public string DataSource { get; set; }
    }
}
