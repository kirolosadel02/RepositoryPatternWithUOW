using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, string[] includes);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, string[] includes);

    }
}
