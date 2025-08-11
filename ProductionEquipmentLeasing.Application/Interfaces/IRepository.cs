using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Application.Interfaces;
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull;
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> Query();
    IQueryable<T> QueryAsNoTracking();
    Task<T?> GetFirstByExpressionAsync(Expression<Func<T, bool>> predicate);
}
