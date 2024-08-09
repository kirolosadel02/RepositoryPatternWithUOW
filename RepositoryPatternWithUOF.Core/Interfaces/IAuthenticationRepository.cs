using Microsoft.AspNetCore.Identity;
using RepositoryPatternWithUOW.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Interfaces
{
    public interface IAuthenticationRepository<TUser> where TUser : class
    {
        Task<AuthenticationModel> RegisterAsync(Register model);
        Task<AuthenticationModel> LoginAsync(Login model);
        Task<String> AddRoleAsync(Role model);
    }
}
