using DogOfTheWeek.Application.Common.Interfaces;
using DogOfTheWeek.Domain.Entities.ApplicationUserAggregate;
using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using DogOfTheWeek.Infrastructure.Common.Interceptors;
using DogOfTheWeek.Infrastructure.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DogOfTheWeek.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
        , AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
        ) : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    public DbSet<DogBreed> DogBreeds { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new DogBreedTypeConfiguration());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}
