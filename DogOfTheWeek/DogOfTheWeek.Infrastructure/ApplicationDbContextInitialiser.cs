using DogOfTheWeek.Domain.Entities.ApplicationUserAggregate;
using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Infrastructure;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager,
                ApplicationDbContext context)
    {
        this._logger = logger;
        this._context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        #region Roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        var dogLoverRole = new IdentityRole("DogLover");

        if (_roleManager.Roles.All(r => r.Name != dogLoverRole.Name))
        {
            await _roleManager.CreateAsync(dogLoverRole);
        }

        var dogOwnerRole = new IdentityRole("DogOwner");

        if (_roleManager.Roles.All(r => r.Name != dogOwnerRole.Name))
        {
            await _roleManager.CreateAsync(dogOwnerRole);
        }
        #endregion

        #region Users
        var administrator = new ApplicationUser { UserName = "administrator@gmail.com", Email = "administrator@gmail.com", EmailConfirmed = true };

        if (_userManager.Users.All(u => u.Email != administrator.Email))
        {
            await _userManager.CreateAsync(administrator, "Admin@123");
            await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }

        var dogLover = new ApplicationUser { UserName = "doglover@gmail.com", Email = "doglover@gmail.com", EmailConfirmed = true };

        if (_userManager.Users.All(u => u.Email != dogLover.Email))
        {
            await _userManager.CreateAsync(dogLover, "User@123");
            await _userManager.AddToRolesAsync(dogLover, new[] { dogLoverRole.Name });
        }

        var dogOwner = new ApplicationUser { UserName = "dogowner@gmail.com", Email = "dogowner@gmail.com", EmailConfirmed = true };

        if (_userManager.Users.All(u => u.Email != dogOwner.Email))
        {
            await _userManager.CreateAsync(dogOwner, "User@123");
            await _userManager.AddToRolesAsync(dogOwner, new[] { dogOwnerRole.Name });
        }
        #endregion

        #region Dog Breeds

        var newBreed = new DogBreed { Name = "German Shepherd", Description = "German Shepherd" };
        if (_context.DogBreeds.All(a => a.Name != newBreed.Name))
        {
            await _context.DogBreeds.AddAsync(newBreed);
        }
        newBreed = new DogBreed { Name = "Bulldog", Description = "Bulldog" };
        if (_context.DogBreeds.All(a => a.Name != newBreed.Name))
        {
            await _context.DogBreeds.AddAsync(newBreed);
        }
        newBreed = new DogBreed { Name = "Labrador Retriever", Description = "Labrador Retriever" };
        if (_context.DogBreeds.All(a => a.Name != newBreed.Name))
        {
            await _context.DogBreeds.AddAsync(newBreed);
        }

        #endregion
        await _context.SaveChangesAsync();
    }
}