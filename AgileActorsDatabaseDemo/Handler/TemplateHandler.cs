using AgileActorsDatabaseDemo.IRepository;
using AgileActorsDatabaseDemo.Query;
using AgileActorsDatabaseDemo.Specification;
using System.Linq.Dynamic.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgileActorsDatabaseDemo.Handler
{
    public class TemplateHandler : IRequestHandler<GetAllTemplatesQuery, List<TemplateResponse>>
    {
        protected readonly ITemplateRepository _templateRepository;

        public TemplateHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }


        public async Task<List<TemplateResponse>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
        {

            var specification = new TemplateSpecification(request.SearchString);

            if (request.OrderBy?.Any() != true)
            {
                var data = await _templateRepository
                    .GetAllAsync(specification.Criteria).ConfigureAwait(false);

                return data.Select(x => new TemplateResponse
                {
                    Description = x.Description,
                    IsActive = x.IsActive,
                    Id = x.Id,
                })
                .ToList();
            }


            var ordering = string.Join(",", request.OrderBy);

            var data1 = await _templateRepository.Entities
                        .OrderBy(ordering)
                        .ToListAsync();

            return new List<TemplateResponse>();
        }
    }
}
