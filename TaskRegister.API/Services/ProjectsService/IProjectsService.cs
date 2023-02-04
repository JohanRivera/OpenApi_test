using TaskRegister.API.Entities.Projects;
using TaskRegister.API.Entities.Response;

namespace TaskRegister.API.Services.ProjectsService
{
    public interface IProjectsService
    {
        Task<CustomResponse<bool>> CreateProject(CreateProjectDto project);
        Task<CustomResponse<List<ProjectDto>>> GetProjects();
        Task<CustomResponse<bool>> UpdateProject(ProjectDto project);
    }
}