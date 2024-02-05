using AgileActorsDemo.Models;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgileActorsDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AggregatorController : ControllerBase
    {

        protected readonly IMediator _mediator;

        public AggregatorController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]AggregatorQuery request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
