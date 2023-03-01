using DogOfTheWeek.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.DogBreed.Commands.UpdateDogBreed;

public class UpdateDogBreedCommandValidator : AbstractValidator<UpdateDogBreedCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDogBreedCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
             .NotEmpty().WithMessage("Id is required.")
             .MustAsync(IsDogBreedAvailable).WithMessage("The specified DogBreed does not exists.");
    }
    public async Task<bool> IsDogBreedAvailable(long id, CancellationToken cancellationToken)
    {
        return await _context.DogBreeds.AnyAsync(x => x.Id == id && x.IsActive);
    }
}