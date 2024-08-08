using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOF.EF;
using RepositoryPatternWithUOW.Core.Interfaces;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(predicate).ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(predicate).ToList();
        }
    }
}
