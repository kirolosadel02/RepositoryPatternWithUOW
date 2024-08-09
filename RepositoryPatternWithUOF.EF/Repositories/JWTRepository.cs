using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class JWTRepository : IJWTRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOptions<JWTSettings> _jwtSettings;

    public JWTRepository(IUnitOfWork unitOfWork,IOptions<JWTSettings>jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _jwtSettings = jwtSettings;
    }

    public async Task<JwtSecurityToken> CreateJwtToken(User user)
    {
        var userClaims = await _unitOfWork.UserManager.GetClaimsAsync(user);
        var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwtSettings.Value.DurationInDays),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
}
