using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Domain.Entities;
public interface IGenericRepository<T> where T : IEntityBase<long>
{
    Task<T> GetAsync<V>(V id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Delete(T entity);
    void Update(T entity);
    IQueryable<T> Where(Func<T, bool> predicate);
    T FirstOrDefault(Func<T, bool> predicate);
    bool Any(Func<T, bool> predicate);
}