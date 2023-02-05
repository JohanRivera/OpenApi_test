using Microsoft.AspNetCore.Mvc;
using TaskRegister.IDP.Entities;
using TaskRegister.IDP.Entities.Response;
using TaskRegister.IDP.Services;

namespace TaskRegister.IDP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserAdministratorController : ControllerBase
    {
        private readonly ILocalUserService _localUserService;

        public UserAdministratorController(
            ILocalUserService localUserService)
        {
            _localUserService = localUserService ??
                throw new ArgumentNullException(nameof(localUserService));
        }

        [HttpPost("CreateUserIdp")]
        public async Task<IActionResult> CreateUserIdp([FromBody] CreateUser createUser)
        {
            CustomResponse<bool> response = await _localUserService.AddUser(createUser);
            return StatusCode(response.StatusCode, response);
        }
    }
}
