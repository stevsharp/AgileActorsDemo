using MediatR;

using System.ComponentModel.DataAnnotations;

namespace AgileActorsDemo.Models
{
    public class AggregatorQuery : IRequest<AggregatorDto>
    {
        [Required]
        public string DataSource { get; set; }

        public string OrderBy { get; set; }
    }
}
