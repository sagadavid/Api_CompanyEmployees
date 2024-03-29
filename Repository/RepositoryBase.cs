﻿using Contracts;//important to add
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext _repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)=>
        _repositoryContext = repositoryContext;
        //RepositoryContext=repositoryContext;//??sample code usage??

        public IQueryable<T> FindAll(bool trackChanges) =>
            //When it’s set to false, we attach the AsNoTracking method to
            //our query to inform EF Core that it doesn’t need to track changes
            //for the required entities. This greatly improves the speed of a query.
            !trackChanges ?
            _repositoryContext.Set<T>().AsNoTracking() :
            _repositoryContext.Set<T>();//https://stackoverflow.com/questions/53469498/difference-between-dbsett-property-and-sett-function-in-ef-core
        //RepositoryContext.Set<T>().AsNoTracking() :
        //RepositoryContext.Set<T>();//sample code ??

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> 
            expression, bool trackChanges) =>
        !trackChanges ? 
            _repositoryContext.Set<T>().Where(expression).AsNoTracking() :
            _repositoryContext.Set<T>().Where(expression);

        public void Create(T entity)=>_repositoryContext.Set<T>().Add(entity);

        public void Update(T entity)=> _repositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => _repositoryContext.Set<T>().Remove(entity);

    }
}
