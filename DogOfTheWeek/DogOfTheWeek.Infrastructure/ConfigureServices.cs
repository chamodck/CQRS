using DogOfTheWeek.Application.Common.Interfaces;
using DogOfTheWeek.Application.Common.Models;
using DogOfTheWeek.Application.Common.Utils;
using DogOfTheWeek.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepository();
        services.AddTransient<IEmailHelper, EmailHelper>();
        services.Configure<AppData>(configuration.GetSection("AppData"));
        return services;
    }
}
