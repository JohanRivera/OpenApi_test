using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using TaskRegister.API.Entities.Projects;
using TaskRegister.API.Services.ProjectsService;

namespace TaskRegister.API.Controllers.Projects
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService ??
                throw new ArgumentNullException(nameof(projectsService));
        }

        // Creación de proyecto
        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject(CreateProjectDto data)
        {
            var response = await _projectsService.CreateProject(data);
            return StatusCode(response.StatusCode, response);
        }

        // Actualizar proyecto
        [HttpPost("UpdateProject")]
        public async Task<IActionResult> UpdateProject(ProjectDto data)
        {
            var response = await _projectsService.UpdateProject(data);
            return StatusCode(response.StatusCode, response);
        }

        // Obtener lista de proyectos
        [HttpGet("GetListProjects")]
        public async Task<IActionResult> GetListProjects()
        {
            var response = await _projectsService.GetProjects();
            return StatusCode(response.StatusCode, response);
        }
    }
}
