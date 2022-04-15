﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAllAsync(Expression<Func<T,bool>> expression) ;
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task AddAsync(T entity , bool isSaveLater=false);
        Task AddRangeAsync(IEnumerable<T> entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

    }
}
