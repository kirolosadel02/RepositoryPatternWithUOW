using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternWithUOW.Services
{
    public interface IAuthService
    {
        Task<AuthenticationModel> RegisterAsync(Register register);
        Task<AuthenticationModel> LoginAsync(Login login);
        Task<string> AddRoleAsync(Role model);
    }
}
