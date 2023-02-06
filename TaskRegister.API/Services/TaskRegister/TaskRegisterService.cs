using NLog;
using TaskRegister.API.DbContexts;
using TaskRegister.API.Entities.Response;
using TaskRegister.API.Entities.TaskRegister;

namespace TaskRegister.API.Services.TaskRegister
{
    public class TaskRegisterService : ITaskRegisterService
    {
        private readonly TaskRegisterContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TaskRegisterService(TaskRegisterContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        // Creación de timeslip
        public async Task<CustomResponse<bool>> CreateTimeslip(TaskRegisterDto data)
        {
            try
            {
                if (data == null)
                    return ResponseCustom(false, false, "Datos invalidos.");

                var toAdd = new ProductionTimeSlip
                {
                    Subject = Guid.NewGuid().ToString(),
                    SubjectUserId = data.SubjectUserId,
                    ProjectId = Guid.Parse(data.ProjectId),
                    AssignedTo = data.AssignedTo,
                    TaskDescription = data.TaskDescription,
                    Comment = data.Comment,
                    Time = data.Time,
                    AssignedDate = Convert.ToDateTime(data.AssignedDate),
                    DateLog = DateTime.Now,
                };

                _context.ProductionTimeSlips.Add(toAdd);

                if (!await SaveChangesAsync())
                    return ResponseCustom(false, false, "Creación fallida.");

                return ResponseCustom(true, true, "Creación exitosa.");
            }
            catch (Exception ex)
            {
                _logger.Error($"TaskRegisterController.CreateTimeslip at: {DateTimeOffset.Now}, error: {ex.Message}, stackTrace: {ex.StackTrace}");
                return ResponseCustom(false, false, "Creación fallida.");
            }
        }

        // Actualización de timeslip
        public async Task<CustomResponse<bool>> UpdateTimeslip(TaskRegisterDto data)
        {
            try
            {
                if (data == null)
                    return ResponseCustom(false, false, "Datos invalidos.");

                var update = _context.ProductionTimeSlips.First(p => p.Subject == data.Subject);

                if (update == null)
                    return ResponseCustom(false, false, "Tarea no existe.");

                update.DateLog = DateTime.Now;
                update.AssignedDate = Convert.ToDateTime(data.AssignedDate);
                update.ProjectId = Guid.Parse(data.ProjectId);
                update.TaskDescription = data.TaskDescription;
                update.Comment = data.Comment;
                update.Time = data.Time;
                update.AssignedTo = data.AssignedTo;
                update.SubjectUserId = data.SubjectUserId;

                if (!await SaveChangesAsync())
                    return ResponseCustom(false, false, "Actualización fallida.");

                return ResponseCustom(true, true, "Actualización exitosa.");
            }
            catch (Exception ex)
            {
                _logger.Error($"TaskRegisterController.UpdateTimeslip at: {DateTimeOffset.Now}, error: {ex.Message}, stackTrace: {ex.StackTrace}");
                return ResponseCustom(false, false, "Actualización fallida.");
            }
        }

        // Obtener timeslip
        public async Task<CustomResponse<List<ReturnTimeslipsDto>>> GetTimeslips(SearchTimeslipDto data)
        {
            try
            {
                if (data == null)
                    return ResponseCustom(new List<ReturnTimeslipsDto>(), false, "Consulta fallida.");

                if (data.IsAll) // Consulta de todos los proyectos
                {
                    var responseAll = from t in _context.ProductionTimeSlips
                                      join p in _context.Projects on t.ProjectId equals p.Id
                                      where t.SubjectUserId == data.SubjectUserId &&
                                            t.AssignedDate >= Convert.ToDateTime(data.DateLimitDown) &&
                                            t.AssignedDate <= Convert.ToDateTime(data.DateLimitUp)
                                      select new ReturnTimeslipsDto
                                      {
                                          SubjectUserId = t.SubjectUserId,
                                          Subject = t.Subject,
                                          AssignedDate = t.AssignedDate.ToString().Substring(0, t.AssignedDate.ToString().IndexOf(' ')),
                                          AssignedTo = t.AssignedTo,
                                          ProjectId = t.ProjectId.ToString(),
                                          ProjectName = p.Project,
                                          TaskDescription = t.TaskDescription,
                                          Time = t.Time / 60 < 1 ? (t.Time < 10 ? $"0h:0{t.Time}m" : $"0h:{t.Time}m")
                                            : (t.Time % 60 < 10 ? $"{Convert.ToInt32(t.Time / 60)}h:0{Convert.ToInt32(t.Time % 60)}m" : $"{Convert.ToInt32(t.Time / 60)}h:{Convert.ToInt32(t.Time % 60)}m"),
                                          Comment = t.Comment
                                      };

                    return ReturnQuery(responseAll);
                }

                // Consulta de un proyecto especifico
                var response = from t in _context.ProductionTimeSlips
                               join p in _context.Projects on t.ProjectId equals p.Id
                               where t.SubjectUserId == data.SubjectUserId &&
                                     t.AssignedDate >= Convert.ToDateTime(data.DateLimitDown) &&
                                     t.AssignedDate <= Convert.ToDateTime(data.DateLimitUp) &&
                                     t.ProjectId == Guid.Parse(data.ProjectId)
                               select new ReturnTimeslipsDto
                               {
                                   SubjectUserId = t.SubjectUserId.ToString(),
                                   Subject = t.Subject,
                                   AssignedDate = t.AssignedDate.ToString().Substring(0, t.AssignedDate.ToString().IndexOf(' ')),
                                   AssignedTo = t.AssignedTo,
                                   ProjectId = t.ProjectId.ToString(),
                                   ProjectName = p.Project,
                                   TaskDescription = t.TaskDescription,
                                   Time = t.Time/60 < 1 ? (t.Time < 10 ? $"0h:0{t.Time}" : $"0h:{t.Time}m") 
                                            : (t.Time % 60 < 10 ? $"{Convert.ToInt32(t.Time/60)}h:0{Convert.ToInt32(t.Time % 60)}m" : $"{Convert.ToInt32(t.Time / 60)}h:{Convert.ToInt32(t.Time % 60)}m"),
                                   Comment = t.Comment
                               };

                return ReturnQuery(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"TaskRegisterController.GetTimeslips at: {DateTimeOffset.Now}, error: {ex.Message}, stackTrace: {ex.StackTrace}");
                return ResponseCustom(new List<ReturnTimeslipsDto>(), false, "Consulta fallida.");
            }
        }

        // Eliminar timeslip
        public async Task<CustomResponse<bool>> DeleteTimeslip(DeleteTimeslipDto data)
        {
            try
            {
                if (data == null)
                    return ResponseCustom(false, false, "Datos invalidos.");

                _context.Remove(_context.ProductionTimeSlips.Single(p => p.Subject == data.Subject));

                if (!await SaveChangesAsync())
                    return ResponseCustom(false, false, "Eliminación fallida.");

                return ResponseCustom(true, true, "Eliminación exitosa.");
            }
            catch (Exception ex)
            {
                _logger.Error($"TaskRegisterController.DeleteTimeslip at: {DateTimeOffset.Now}, error: {ex.Message}, stackTrace: {ex.StackTrace}");
                return ResponseCustom(false, false, "Eliminación fallida.");
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

        private static CustomResponse<List<ReturnTimeslipsDto>> ReturnQuery(IQueryable<ReturnTimeslipsDto> response)
        {
            if (response == null)
                return ResponseCustom(new List<ReturnTimeslipsDto>(), true, "Consulta exitosa.");

            return ResponseCustom(response.ToList(), true, "Consulta exitosa.");
        }

        #endregion
    }
}
