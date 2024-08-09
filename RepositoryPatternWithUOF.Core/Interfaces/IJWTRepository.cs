using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternWithUOW.Core.Interfaces
{
    public interface IJWTRepository
    {
        Task<JwtSecurityToken> CreateJwtToken(User user);
    }
}
