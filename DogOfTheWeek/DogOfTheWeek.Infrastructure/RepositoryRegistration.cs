using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using DogOfTheWeek.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Infrastructure;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddTransient<IDogBreedRepository, DogBreedRepository>();
        return services;
    }
}
