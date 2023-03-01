using DogOfTheWeek.Application.Handlers.DogBreed.Models;
using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.DogBreed.Commands.CreateDogBreed;

public class CreateDogBreedCommand : DogBreedRequestDto, IRequest<long>
{
    [JsonIgnore]
    public override long Id { get; set; }
}
public class CreateDogBreedCommandHandler : IRequestHandler<CreateDogBreedCommand, long>
{
    private readonly IDogBreedRepository _dogBreedRepository;

    public CreateDogBreedCommandHandler(IDogBreedRepository dogBreedRepository)
    {
        _dogBreedRepository = dogBreedRepository;
    }

    public async Task<long> Handle(CreateDogBreedCommand request, CancellationToken cancellationToken)
    {
        var entity = new DogOfTheWeek.Domain.Entities.DogBreedAggregate.DogBreed()
        {
            Name = request.Name,
            Description = request.Description
        };
        await _dogBreedRepository.CreateAsync(entity);
        return entity.Id;
    }
}