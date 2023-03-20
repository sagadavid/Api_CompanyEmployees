using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    /*In asynchronous programming, we have three return types:
    * Task<TResult>, for an async method that returns a value.
    * Task, for an async method that does not return a value.
    * void, which we can use for an event handler.
    *      but we dont async in repobase class, to keep available for syncronous methods, sometimes async takes more time than sync*/

    public interface IRepositoryBase<T>//methods defined in repositorybase !!
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> 
            expression, bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
