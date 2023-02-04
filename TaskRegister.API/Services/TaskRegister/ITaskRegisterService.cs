using TaskRegister.API.Entities.Response;
using TaskRegister.API.Entities.TaskRegister;

namespace TaskRegister.API.Services.TaskRegister
{
    public interface ITaskRegisterService
    {
        Task<CustomResponse<bool>> CreateTimeslip(TaskRegisterDto data);
        Task<CustomResponse<bool>> DeleteTimeslip(DeleteTimeslipDto data);
        Task<CustomResponse<List<ReturnTimeslipsDto>>> GetTimeslips(SearchTimeslipDto data);
        Task<CustomResponse<bool>> UpdateTimeslip(TaskRegisterDto data);
    }
}