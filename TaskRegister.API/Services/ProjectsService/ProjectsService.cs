using NLog;
using TaskRegister.API.DbContexts;
using TaskRegister.API.Entities.Projects;
using TaskRegister.API.Entities.Response;

namespace TaskRegister.API.Services.ProjectsService
{
    public class ProjectsService : IProjectsService
    {
        private readonly TaskRegisterContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ProjectsService(TaskRegisterContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<CustomResponse<bool>> CreateProject(CreateProjectDto project)
        {
            try
            {
                if (project == null)
                    return ResponseCustom(false, false, "Proyecto invalido.");

                if (_context.Projects.Any(p => p.Project == project.ProjectName))
                    return ResponseCustom(false, false, "Proyecto ya existe.");

                var projectToAdd = new BaseProjects
                {
                    Subject = Guid.NewGuid().ToString(),
                    Project = project.ProjectName,
                    Active = true
                };

                _context.Projects.Add(projectToAdd);

                if (!await SaveChangesAsync())
                    return ResponseCustom(false, false, "Creación fallida.");

                return ResponseCustom(true, true, "Creación exitosa.");
            }
            catch (Exception ex)
            {
                _logger.Error($"ProjectController.CreateProject at: {DateTimeOffset.Now}, error: {ex.Message}, stackTrace: {ex.StackTrace}");
                return ResponseCustom(false, false, "Creación fallida.");
            }
        }

        public async Task<CustomResponse<bool>> UpdateProject(ProjectDto project)
        {
            try
            {
                if (project == null)
                    return ResponseCustom(false, false, "Proyecto invalido.");

                if (_context.Projects.Any(p => p.Project == project.ProjectName && p.Subject != project.Subject))
                    return ResponseCustom(false, false, "Proyecto ya existe.");

                var update = _context.Projects.First(p => p.Subject == project.Subject);
                update.Project = project.ProjectName;
                update.Active = project.Active;

                if (!await SaveChangesAsync())
                    return ResponseCustom(false, false, "Actualización fallida.");

                return ResponseCustom(true, true, "Actualización exitosa.");
            }
            catch (Exception ex)
            {
                _logger.Error($"ProjectController.UpdateProject at: {DateTimeOffset.Now}, error: {ex.Message}, stackTrace: {ex.StackTrace}");
                return ResponseCustom(false, false, "Actualización fallida.");
            }
        }

        public async Task<CustomResponse<List<ProjectDto>>> GetProjects()
        {
            try
            {
                var projects = _context.Projects.Select(r => new ProjectDto()
                {
                    Id = r.Id.ToString(),
                    Subject = r.Subject,
                    ProjectName = r.Project,
                    Active = r.Active
                }).ToList();

                return ResponseCustom(projects, true, "Consulta exitosa.");
            }
            catch (Exception ex)
            {
                _logger.Error($"ProjectController.GetProjects at: {DateTimeOffset.Now}, error: {ex.Message}, stackTrace: {ex.StackTrace}");
                return ResponseCustom(new List<ProjectDto>(), false, "Consulta fallida.");
            }
        }

        #region Funciones privadas

        private static CustomResponse<T> ResponseCustom<T>(T content, bool isSuccess, string msj)
        {
            return new CustomResponse<T> { IsSuccessful = isSuccess, Message = msj, Content = content, StatusCode = 200 };
        }

        private async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        #endregion
    }
}
