using AgileActorsDatabaseDemo.Model;

namespace AgileActorsDatabaseDemo.IRepository
{
    public interface ITemplateRepository : IRepository<Template, int>
    {

    }

    public class TemplateRepository : Repository<Template, int>, ITemplateRepository
    {
        public TemplateRepository(BaseDbContext dbContext) : base(dbContext)
        {

        }
    }
}
