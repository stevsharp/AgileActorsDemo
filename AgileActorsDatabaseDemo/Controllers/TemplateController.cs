using AgileActorsDatabaseDemo.Query;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgileActorsDatabaseDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {

        protected readonly IMediator _mediator;

        public TemplateController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var products = await _mediator.Send(new GetAllTemplatesQuery(pageNumber, pageSize, searchString, orderBy));

            return Ok(products);
        }

    }
}
