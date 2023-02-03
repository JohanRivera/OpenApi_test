using TaskRegister.IDP.Entities;
using TaskRegister.IDP.Entities.Response;

namespace TaskRegister.IDP.Services
{
    public interface ILocalUserService
    {
        Task<bool> ValidateCredentialsAsync(
             string userName,
             string password);

        Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(
            string subject);

        Task<User> GetUserByUserNameAsync(
            string userName);

        Task<User> GetUserBySubjectAsync(
            string subject);

        Task<CustomResponse<bool>> AddUser(
            CreateUser createUser);

        Task<bool> IsUserActive(
            string subject);

        Task<bool> SaveChangesAsync();
    }
}
