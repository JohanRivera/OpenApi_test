using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskRegister.API.Entities.TaskRegister;
using TaskRegister.API.Services.TaskRegister;

namespace TaskRegister.API.Controllers.TaskRegister
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskRegisterController : ControllerBase
    {
        private readonly ITaskRegisterService _taskRegisterService;

        public TaskRegisterController(ITaskRegisterService taskRegisterService)
        {
            _taskRegisterService = taskRegisterService ??
                throw new ArgumentNullException(nameof(taskRegisterService));
        }

        // Creación del registro de la tarea
        [HttpPost("CreateTimeslip")]
        public async Task<IActionResult> CreateTimeslip(TaskRegisterDto data)
        {
            var response = await _taskRegisterService.CreateTimeslip(data);
            return StatusCode(response.StatusCode, response);
        }

        // Actualizar registro de la tarea
        [HttpPost("UpdateTimeslip")]
        public async Task<IActionResult> UpdateTimeslip(TaskRegisterDto data)
        {
            var response = await _taskRegisterService.UpdateTimeslip(data);
            return StatusCode(response.StatusCode, response);
        }

        // Obtener lista de los registros de la tarea
        [HttpPost("GetListTimeslips")]
        public async Task<IActionResult> GetListTimeslips(SearchTimeslipDto data)
        {
            var response = await _taskRegisterService.GetTimeslips(data);
            return StatusCode(response.StatusCode, response);
        }

        // Obtener lista de los registros de la tarea
        [HttpPost("DeleteTimeslip")]
        public async Task<IActionResult> DeleteTimeslip([FromForm] DeleteTimeslipDto data)
        {
            var response = await _taskRegisterService.DeleteTimeslip(data);
            return StatusCode(response.StatusCode, response);
        }
    }
}
