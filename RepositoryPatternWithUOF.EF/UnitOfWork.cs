using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RepositoryPatternWithUOF.Core.Models;
using RepositoryPatternWithUOF.EF;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.EF.Repositories;
using System;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly JWTSettings _jwtSettings;

    public IBaseRepository<Author> Authors { get; private set; }
    public IBaseRepository<Book> Books { get; private set; }
    public IAuthenticationRepository<User> Users { get; private set; }
    public UserManager<User> UserManager { get; private set; }
    public RoleManager<IdentityRole> RoleManager { get; private set; }
    public JWTSettings JwtSettings => _jwtSettings; // Implement the property

    public UnitOfWork(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWTSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
        Authors = new BaseRepository<Author>(_context);
        Books = new BaseRepository<Book>(_context);
        UserManager = userManager;
        RoleManager = roleManager;
        Users = new JWTRepository(this); // Pass UnitOfWork itself
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
