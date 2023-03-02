using DogOfTheWeek.Application.Common.Interfaces;
using DogOfTheWeek.Application.Common.Models;
using DogOfTheWeek.Application.Common.Utils;
using DogOfTheWeek.Infrastructure;
using DogOfTheWeek.Infrastructure.Common.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
        var config = builder.Build();
        string connectionString = config["ConnectionStrings:Database"];

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepository();
        services.AddTransient<IEmailHelper, EmailHelper>();
        services.Configure<AppData>(configuration.GetSection("AppData"));
        return services;
    }
}
