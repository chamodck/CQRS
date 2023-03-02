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

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
            .MustAsync(IsDogBreedNameNotAvailbale).WithMessage("The specified Name already exists.");

        RuleFor(v => v.Description)
             .NotEmpty().WithMessage("Description is required.")
             .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
    }
    public async Task<bool> IsDogBreedAvailable(long id, CancellationToken cancellationToken)
    {
        return await _context.DogBreeds.AnyAsync(x => x.Id == id && x.IsActive);
    }

    public async Task<bool> IsDogBreedNameNotAvailbale(UpdateDogBreedCommand model, string name, CancellationToken cancellationToken)
    {
        var available = await _context.DogBreeds.AnyAsync(x => x.Name == name && x.Id != model.Id && x.IsActive, cancellationToken);
        return !available;
    }
}