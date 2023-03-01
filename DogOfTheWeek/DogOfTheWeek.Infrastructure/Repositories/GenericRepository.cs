
using DogOfTheWeek.Domain;
using DogOfTheWeek.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity<long>
{
    protected readonly ApplicationDbContext dbContext;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
    }

    public void Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
    }

    public IQueryable<T> Where(Func<T, bool> predicate)
    {
        return dbContext.Set<T>().Where(predicate).AsQueryable<T>();
    }

    public async Task<T> GetAsync<V>(V id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await dbContext.Set<T>().Where(a => a.IsActive).ToListAsync();
    }

    public void Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
    }

    public T FirstOrDefault(Func<T, bool> predicate)
    {
        return dbContext.Set<T>().FirstOrDefault(predicate);
    }

    public bool Any(Func<T, bool> predicate)
    {
        return dbContext.Set<T>().Any(predicate);
    }
}
