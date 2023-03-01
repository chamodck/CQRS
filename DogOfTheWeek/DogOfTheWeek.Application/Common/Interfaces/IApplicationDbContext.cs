using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<DogBreed> DogBreeds { get; }
}
