using AgileActorsDatabaseDemo.Model;

namespace AgileActorsDatabaseDemo.Specification
{
    public class TemplateSpecification : BaseSpecification<Template>
    {
        public TemplateSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Description != null && (p.Description.Contains(searchString) || p.Description.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Description != null;
            }
        }
    }
}
