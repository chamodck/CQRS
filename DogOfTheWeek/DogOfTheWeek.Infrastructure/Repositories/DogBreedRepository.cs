using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using DogOfTheWeek.Infrastructure;
using DogOfTheWeek.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Infrastructure.Repositories;

public class DogBreedRepository : GenericRepository<DogBreed>, IDogBreedRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DogBreedRepository> _logger;

    public DogBreedRepository(ApplicationDbContext context, ILogger<DogBreedRepository> logger) : base(context)
    {
        this._context = context;
        this._logger = logger;
    }

    public async Task<DogBreed> GetByIdAsync(long id)
    {
        var dbRecord = await _context.DogBreeds
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        return dbRecord;
    }

    public async Task<DogBreed> CreateAsync(DogBreed entity)
    {
        _context.DogBreeds.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<DogBreed> UpdateAsync(DogBreed entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.DogBreeds.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}