using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Infrastructure.EntityConfigurations;

class DogBreedTypeConfiguration : IEntityTypeConfiguration<DogBreed>
{
    public DogBreedTypeConfiguration()
    {
    }

    public void Configure(EntityTypeBuilder<DogBreed> builder)
    {
        builder.Property(a => a.Name).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Description).HasMaxLength(1000).IsRequired();
    }
}