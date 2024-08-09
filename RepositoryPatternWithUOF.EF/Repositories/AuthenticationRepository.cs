using Microsoft.IdentityModel.Tokens;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthenticationRepository : IAuthenticationRepository<User>
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthenticationModel> RegisterAsync(Register model)
    {
        if (await _unitOfWork.UserManager.FindByEmailAsync(model.Email) is not null)
            return new AuthenticationModel { Message = "Email already exists!" };

        if (await _unitOfWork.UserManager.FindByNameAsync(model.UserName) is not null)
            return new AuthenticationModel { Message = "Username already exists!" };

        var user = new User
        {
            Email = model.Email,
            UserName = model.UserName,
            FirstName = model.FirtName,
            LastName = model.LastName
        };

        var result = await _unitOfWork.UserManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthenticationModel { Message = errors };
        }

        await _unitOfWork.UserManager.AddToRoleAsync(user, "User");

        var jwtToken = await _unitOfWork.JwtRepository.CreateJwtToken(user);


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

    public async Task<AuthenticationModel> LoginAsync(Login model)
    {
        var user = await _unitOfWork.UserManager.FindByEmailAsync(model.Email);
        if (user == null || !await _unitOfWork.UserManager.CheckPasswordAsync(user, model.Password))
            return new AuthenticationModel { Message = "Email or Password is incorrect!" };

        var jwtToken = await _unitOfWork.JwtRepository.CreateJwtToken(user);

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

    public async Task<string> AddRoleAsync(Role model)
    {
        var user = await _unitOfWork.UserManager.FindByIdAsync(model.UserId);

        if (user is null || !await _unitOfWork.RoleManager.RoleExistsAsync(model.RoleName))
            return "Invalid User ID or Role";

        if(await _unitOfWork.UserManager.IsInRoleAsync(user, model.RoleName))
            return "User already has this role";

        var result = await _unitOfWork.UserManager.AddToRoleAsync(user, model.RoleName);

        return result.Succeeded ? string.Empty : "Something went wrong!";
    }
}
