using DogOfTheWeek.Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.DogBreed.Commands.CreateDogBreed;

public class CreateDogBreedCommandValidator : AbstractValidator<CreateDogBreedCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateDogBreedCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
             .NotEmpty().WithMessage("Name is required.")
             .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(v => v.Description)
             .NotEmpty().WithMessage("Description is required.")
             .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
    }
}