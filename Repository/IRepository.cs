using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Repository
{
    public interface IRepository<T>
    {
        Task<T> CreateAsync(T document);
        Task<T> GetByIdAsync(Guid id);
        Task<T> ReplaceAsync(T document);

        DeleteResult Delete(T document);
        Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression);
        Task<List<T>> GetAll();
    }
}