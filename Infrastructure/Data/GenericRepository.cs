using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(FantasyFootballContext db):IGenericRepository<T> where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(int id)
    {
        return await db.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T?>> ListAllAsync()
    {
        return await db.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T?>> GetListAsyncWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TResult>> GetListAsyncWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public void Add(T entity)
    {
        db.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        db.Set<T>().Attach(entity);
        db.Entry(entity).State = EntityState.Modified;
    }

    public void Remove(T entity)
    {
        db.Set<T>().Remove(entity);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await db.SaveChangesAsync() > 0;
    }

    public bool Exists(int id)
    {
        return db.Set<T>().Any(t => t.Id == id);
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        var query = db.Set<T>().AsQueryable();
        query = spec.ApplyCriteria(query);
        return await query.CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(db.Set<T>().AsQueryable(), spec);
    }
    
    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
    {
        return SpecificationEvaluator<T>.GetQuery<T,TResult>(db.Set<T>().AsQueryable(), spec);
    }
    
    
    
    
    
    
}