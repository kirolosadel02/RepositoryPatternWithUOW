using Microsoft.AspNetCore.Identity;
using RepositoryPatternWithUOF.Core.Models;
using RepositoryPatternWithUOW.Core.Models;
using System;

namespace RepositoryPatternWithUOW.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Author> Authors { get; }
        IBaseRepository<Book> Books { get; }
        IAuthenticationRepository<User> Users { get; }
        UserManager<User> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        JWTSettings JwtSettings { get; } // Add this property
        int Complete();
    }
}
