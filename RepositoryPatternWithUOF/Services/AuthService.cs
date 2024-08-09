using Microsoft.Extensions.Options;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Services;
using System.IdentityModel.Tokens.Jwt;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthenticationModel> RegisterAsync(Register register)
    {
        return await _unitOfWork.Users.RegisterAsync(register);
    }

    public async Task<AuthenticationModel> LoginAsync(Login login) 
    {
        return await _unitOfWork.Users.LoginAsync(login);
    }

    public async Task<string> AddRoleAsync(Role model)
    {
        return await _unitOfWork.Users.AddRoleAsync(model);
    }
}
