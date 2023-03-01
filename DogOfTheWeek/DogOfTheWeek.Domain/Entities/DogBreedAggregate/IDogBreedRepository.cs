using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Domain.Entities.DogBreedAggregate;

public interface IDogBreedRepository : IGenericRepository<DogBreed>
{
    public Task<DogBreed> GetByIdAsync(long id);
    public Task<DogBreed> CreateAsync(DogBreed request);
    public Task<DogBreed> UpdateAsync(DogBreed request);
}