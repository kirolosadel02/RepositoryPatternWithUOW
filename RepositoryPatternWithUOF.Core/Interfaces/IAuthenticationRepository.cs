using RepositoryPatternWithUOW.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Interfaces
{
    public interface IAuthenticationRepository<TUser> where TUser : class
    {
        Task<AuthenticationModel> RegisterAsync(Register register);
        Task<AuthenticationModel> LoginAsync(string email, string password);
        Task<JwtSecurityToken> CreateJwtToken(User user);
    }
}
