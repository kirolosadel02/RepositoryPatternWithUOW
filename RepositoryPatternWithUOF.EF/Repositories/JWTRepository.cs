using Microsoft.IdentityModel.Tokens;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JWTRepository : IAuthenticationRepository<User>
{
    private readonly IUnitOfWork _unitOfWork;

    public JWTRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

           var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_unitOfWork.JwtSettings.Key));
           var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

           var jwtSecurityToken = new JwtSecurityToken(
               issuer: _unitOfWork.JwtSettings.Issuer,
               audience: _unitOfWork.JwtSettings.Audience,
               claims: claims,
               expires: DateTime.Now.AddDays(_unitOfWork.JwtSettings.DurationInDays),
               signingCredentials: signingCredentials);

           return jwtSecurityToken;
    }

    public async Task<AuthenticationModel> RegisterAsync(Register register)
    {
        if (await _unitOfWork.UserManager.FindByEmailAsync(register.Email) is not null)
            return new AuthenticationModel { Message = "Email already exists!" };

        if (await _unitOfWork.UserManager.FindByNameAsync(register.UserName) is not null)
            return new AuthenticationModel { Message = "Username already exists!" };

        var user = new User
        {
            Email = register.Email,
            UserName = register.UserName,
            FirstName = register.FirtName,
            LastName = register.LastName
        };

        var result = await _unitOfWork.UserManager.CreateAsync(user, register.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthenticationModel { Message = errors };
        }

        await _unitOfWork.UserManager.AddToRoleAsync(user, "User");

        var jwtToken = await CreateJwtToken(user);

        return new AuthenticationModel
        {
            IsAuthenticated = true,
            UserName = user.UserName,
            Email = user.Email,
            Roles = new[] { "User" }.ToList(),
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            ExpiresOn = jwtToken.ValidTo
        };
    }

    public async Task<AuthenticationModel> LoginAsync(string email, string password)
    {
        var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
        if (user == null || !await _unitOfWork.UserManager.CheckPasswordAsync(user, password))
            return new AuthenticationModel { Message = "Email or Password is incorrect!" };

        var jwtToken = await CreateJwtToken(user);

        return new AuthenticationModel
        {
            IsAuthenticated = true,
            UserName = user.UserName,
            Email = user.Email,
            Roles = (await _unitOfWork.UserManager.GetRolesAsync(user)).ToList(),
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            ExpiresOn = jwtToken.ValidTo
        };
    }
}
