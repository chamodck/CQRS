using DogOfTheWeek.Application.Handlers.DogBreed.Models;
using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.DogBreed.Commands.UpdateDogBreed;

public class UpdateDogBreedCommand : DogBreedRequestDto, IRequest
{
}

public class UpdateDogBreedCommandHandler : IRequestHandler<UpdateDogBreedCommand>
{
    private readonly IDogBreedRepository _dogBreedRepository;
    private readonly IMediator _mediator;
    public UpdateDogBreedCommandHandler(IDogBreedRepository dogBreedRepository
        , IMediator mediator)
    {
        this._dogBreedRepository = dogBreedRepository;
        this._mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateDogBreedCommand request, CancellationToken cancellationToken)
    {
        var dbRecord = await _dogBreedRepository.GetByIdAsync(request.Id);

        dbRecord.Name = request.Name;
        dbRecord.Description = request.Description;

        await _dogBreedRepository.UpdateAsync(dbRecord);

        return Unit.Value;
    }
}