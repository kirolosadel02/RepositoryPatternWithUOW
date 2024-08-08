using Microsoft.Extensions.Options;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Services;
using System.IdentityModel.Tokens.Jwt;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JWTSettings _jwtSettings;

    public AuthService(IUnitOfWork unitOfWork, IOptions<JWTSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthenticationModel> RegisterAsync(Register register)
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("JWT key is not configured.");
        }
        if (await _unitOfWork.UserManager.FindByEmailAsync(register.Email) is not null)
            return new AuthenticationModel { Message = "Email already exists!" };
        if (await _unitOfWork.UserManager.FindByNameAsync(register.Email) is not null)
            return new AuthenticationModel { Message = "UserName already exists!" };

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

        var jwtToken = await _unitOfWork.Users.CreateJwtToken(user);

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
}
