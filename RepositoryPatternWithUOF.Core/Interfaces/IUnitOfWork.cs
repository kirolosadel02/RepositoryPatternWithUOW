using Microsoft.AspNetCore.Identity;
using RepositoryPatternWithUOF.Core.Models;
using RepositoryPatternWithUOW.Core.Models;
using System;

namespace RepositoryPatternWithUOW.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories for managing entities
        IBaseRepository<Author> Authors { get; }
        IBaseRepository<Book> Books { get; }

        // Authentication and JWT token repositories
        IAuthenticationRepository<User> Users { get; }
        IJWTRepository JwtRepository { get; }

        // Identity management
        UserManager<User> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }

        // Save changes to the database
        int Complete();
    }
}
