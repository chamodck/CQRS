using DogOfTheWeek.Application.Handlers.DogBreed.Models;
using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.DogBreed.Commands.DeleteDogBreed;

public record DeleteDogBreedCommand(long Id) : IRequest;

public class DeleteDogBreedCommandHandler : IRequestHandler<DeleteDogBreedCommand>
{
    private readonly IDogBreedRepository _dogBreedRepository;
    private readonly IMediator _mediator;
    public DeleteDogBreedCommandHandler(IDogBreedRepository dogBreedRepository
        , IMediator mediator)
    {
        this._dogBreedRepository = dogBreedRepository;
        this._mediator = mediator;
    }

    public async Task<Unit> Handle(DeleteDogBreedCommand request, CancellationToken cancellationToken)
    {
        var dbRecord = await _dogBreedRepository.GetByIdAsync(request.Id);

        dbRecord.IsActive = false;

        await _dogBreedRepository.UpdateAsync(dbRecord);

        return Unit.Value;
    }
}