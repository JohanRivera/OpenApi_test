﻿using TaskRegister.API.DbContexts;
using TaskRegister.API.Entities.Response;
using TaskRegister.API.Entities.TaskRegister;

namespace TaskRegister.API.Services.TaskRegister
{
    public class TaskRegisterService : ITaskRegisterService
    {
        private readonly TaskRegisterContext _context;

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
                    UserId = Guid.Parse(data.UserId),
                    ProjectId = Guid.Parse(data.ProjectId),
                    AssignedTo = data.AssignedTo,
                    TaskDescription = data.TaskDescription,
                    Comment = data.Comment,
                    Time = data.Time,
                    AssignedDate = data.AssignedDate,
                    DateLog = DateTime.Now,
                };

                _context.ProductionTimeSlips.Add(toAdd);

                if (!await SaveChangesAsync())
                    return ResponseCustom(false, false, "Creación fallida.");

                return ResponseCustom(true, true, "Creación exitosa.");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
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
                update.AssignedDate = data.AssignedDate;
                update.ProjectId = Guid.Parse(data.ProjectId);
                update.TaskDescription = data.TaskDescription;
                update.Comment = data.Comment;
                update.Time = data.Time;
                update.AssignedTo = data.AssignedTo;
                update.UserId = Guid.Parse(data.UserId);

                if (!await SaveChangesAsync())
                    return ResponseCustom(false, false, "Actualización fallida.");

                return ResponseCustom(true, true, "Actualización exitosa.");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
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
                                      where t.UserId == Guid.Parse(data.UserId) &&
                                            t.AssignedDate >= Convert.ToDateTime(data.DateLimitDown) &&
                                            t.AssignedDate <= Convert.ToDateTime(data.DateLimitUp)
                                      select new ReturnTimeslipsDto
                                      {
                                          UserId = t.UserId.ToString(),
                                          Subject = t.Subject,
                                          AssignedDate = Convert.ToDateTime(t.AssignedDate),
                                          AssignedTo = t.AssignedTo,
                                          ProjectId = t.ProjectId.ToString(),
                                          ProjectName = p.Project,
                                          TaskDescription = t.TaskDescription,
                                          Time = t.Time,
                                          Comment = t.Comment
                                      };

                    /*var response = _context.ProductionTimeSlips
                        .Where(p => p.UserId == Guid.Parse(data.UserId)
                                && p.AssignedDate >= Convert.ToDateTime(data.DateLimitDown)
                                && p.AssignedDate <= Convert.ToDateTime(data.DateLimitUp))
                        .Select(r => new ReturnTimeslipsDto
                        {
                            UserId = r.UserId.ToString(),
                            Subject = r.Subject,
                            AssignedDate = Convert.ToDateTime(r.AssignedDate),
                            AssignedTo = r.AssignedTo,
                            ProjectId = r.ProjectId.ToString(),
                            Comment= r.Comment
                        }).ToList();*/

                    if (responseAll == null)
                        return ResponseCustom(new List<ReturnTimeslipsDto>(), true, "Consulta exitosa.");

                    return ResponseCustom(responseAll.ToList(), true, "Consulta exitosa.");
                }

                // Consulta de un proyecto especifico
                var response = from t in _context.ProductionTimeSlips
                               join p in _context.Projects on t.ProjectId equals p.Id
                               where t.UserId == Guid.Parse(data.UserId) &&
                                     t.AssignedDate >= Convert.ToDateTime(data.DateLimitDown) &&
                                     t.AssignedDate <= Convert.ToDateTime(data.DateLimitUp) &&
                                     t.ProjectId == Guid.Parse(data.ProjectId)
                               select new ReturnTimeslipsDto
                               {
                                   UserId = t.UserId.ToString(),
                                   Subject = t.Subject,
                                   AssignedDate = Convert.ToDateTime(t.AssignedDate),
                                   AssignedTo = t.AssignedTo,
                                   ProjectId = t.ProjectId.ToString(),
                                   ProjectName = p.Project,
                                   TaskDescription = t.TaskDescription,
                                   Time = t.Time,
                                   Comment = t.Comment
                               };

                if (response == null)
                    return ResponseCustom(new List<ReturnTimeslipsDto>(), true, "Consulta exitosa.");

                return ResponseCustom(response.ToList(), true, "Consulta exitosa.");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
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
                Console.Write(ex.Message);
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

        #endregion
    }
}