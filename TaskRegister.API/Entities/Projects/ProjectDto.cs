namespace TaskRegister.API.Entities.Projects
{
    public class CreateProjectDto
    {
        public string ProjectName { get; set; }
    }

    public class ProjectDto
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string ProjectName { get; set; }
        public bool Active { get; set; }
    }
}
