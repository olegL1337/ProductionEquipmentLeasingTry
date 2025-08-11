using Microsoft.EntityFrameworkCore;
using ProductionEquipmentLeasing.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly EquipmentLeasingContext _context;

    public Repository(EquipmentLeasingContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetFirstByExpressionAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>()
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
       await  _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>()
            .AnyAsync(predicate);
    }

    public IQueryable<T> Query()
    {
        return _context.Set<T>();
    }

    public IQueryable<T> QueryAsNoTracking()
    {
        return _context.Set<T>().AsNoTracking();
    }
}
