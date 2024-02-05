using MediatR;

namespace AgileActorsDatabaseDemo.Query
{
    public class GetAllTemplatesQuery : IRequest<TemplateResponse>
    {
        public int PageNumber { get; }
        public int PageSize { get;  }
        public string SearchString { get; }
        public string[] OrderBy { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        public GetAllTemplatesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }
}
