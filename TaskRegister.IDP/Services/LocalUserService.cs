using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskRegister.IDP.Entities;
using TaskRegister.IDP.DbContexts;
using TaskRegister.IDP.Entities.Response;
using IdentityModel;
using NLog;

namespace TaskRegister.IDP.Services
{
    public class LocalUserService : ILocalUserService
    {
        private readonly IdentityDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public LocalUserService(
            IdentityDbContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _passwordHasher = passwordHasher ??
                throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<bool> IsUserActive(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                return false;
            }

            var user = await GetUserBySubjectAsync(subject);

            if (user == null)
            {
                return false;
            }

            return user.Active;
        }

        public async Task<bool> ValidateCredentialsAsync(string userName,
          string password)
        {
            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = await GetUserByUserNameAsync(userName);

            if (user == null)
            {
                return false;
            }

            if (!user.Active)
            {
                return false;
            }

            // Validate credentials
            // return (user.Password == password);
            var verificationResult = _passwordHasher.VerifyHashedPassword(
                user, user.Password, password);

            return verificationResult == PasswordVerificationResult.Success;
        } 

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return await _context.Users
                 .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            return await _context.UserClaims.Where(u => u.User.Subject == subject).ToListAsync();
        }

        public async Task<User> GetUserBySubjectAsync(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            return await _context.Users.FirstOrDefaultAsync(u => u.Subject == subject);
        }

        public async Task<CustomResponse<bool>> AddUser(CreateUser createUser)
        {
            try
            {
                if (createUser == null)
                    return ResponseCustom(false, false, "Sin información del usuario.");

                if (_context.Users.Any(u => u.UserName == createUser.UserName))
                    return ResponseCustom(false, false, "Nombre de usuario ya existe.");


                var userToAdd = new User
                {
                    UserName = createUser.UserName,
                    Password = createUser.Password,
                    Subject = Guid.NewGuid().ToString(),
                    Active = true,
                    Claims = new List<UserClaim>()
                    {
                        new UserClaim
                        {
                            Type = JwtClaimTypes.GivenName,
                            Value = createUser.FirstName
                        },
                        new UserClaim
                        {
                            Type = JwtClaimTypes.FamilyName,
                            Value = createUser.LastName
                        }
                    }
                };

                // Hash password
                userToAdd.Password = _passwordHasher.HashPassword(userToAdd, userToAdd.Password);

                _context.Users.Add(userToAdd);

                if(!await SaveChangesAsync())
                    return ResponseCustom(false, false, "Creación fallida.");

                return ResponseCustom(true, true, "Creación exitosa.");

            }
            catch(Exception ex)
            {
                _logger.Error($"UserAdministratorController.AddUser at: {DateTimeOffset.Now}, error: {ex.Message}, stackTrace: {ex.StackTrace}");
                return ResponseCustom(false, false, "Creación de usuario fallido.");
            }
            
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        #region Funciones privadas

        private static CustomResponse<T> ResponseCustom<T>(T content, bool isSuccess, string msj)
        {
            return new CustomResponse<T> { IsSuccessful = isSuccess, Message = msj, Content = content, StatusCode = 200 };
        }

        #endregion
    }
}
