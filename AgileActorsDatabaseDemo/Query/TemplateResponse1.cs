namespace AgileActorsDatabaseDemo
{
    public class TemplateResponse
    {
        public string Description { get; }
        public int IsActive { get; }

        public TemplateResponse(string description, int isActive)
        {
            Description = description;
            IsActive = isActive;
        }
    }
}