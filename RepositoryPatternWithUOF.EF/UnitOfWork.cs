using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RepositoryPatternWithUOF.Core.Models;
using RepositoryPatternWithUOF.EF;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.EF.Repositories;
using System;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IBaseRepository<Author> Authors { get; private set; }
    public IBaseRepository<Book> Books { get; private set; }
    public IAuthenticationRepository<User> Users { get; private set; }
    public IJWTRepository JwtRepository { get; private set; }
    public UserManager<User> UserManager { get; private set; }
    public RoleManager<IdentityRole> RoleManager { get; private set; }

    public UnitOfWork(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWTSettings> jwtSettings)
    {
        _context = context;
        Authors = new BaseRepository<Author>(_context);
        Books = new BaseRepository<Book>(_context);
        UserManager = userManager;
        RoleManager = roleManager;
        JwtRepository = new JWTRepository(this,jwtSettings); // Pass JWTSettings to JWTRepository
        Users = new AuthenticationRepository(this); // Initialize AuthenticationRepository
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
