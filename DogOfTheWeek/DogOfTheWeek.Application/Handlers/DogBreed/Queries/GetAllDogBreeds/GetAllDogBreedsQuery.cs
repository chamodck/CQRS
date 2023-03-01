using AutoMapper;
using DogOfTheWeek.Application.Handlers.DogBreed.Models;
using DogOfTheWeek.Domain.Entities.DogBreedAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.DogBreed.Queries.GetAllDogBreeds;

public record GetAllDogBreedsQuery : IRequest<DogBreedsVM>
{
}

public class GetAllDogBreedsQueryHandler : IRequestHandler<GetAllDogBreedsQuery, DogBreedsVM>
{
    private readonly IMapper _mapper;
    private readonly IDogBreedRepository _dogBreedRepository;

    public GetAllDogBreedsQueryHandler(IMapper mapper,
        IDogBreedRepository dogBreedRepository)
    {
        this._mapper = mapper;
        this._dogBreedRepository = dogBreedRepository;
    }

    public async Task<DogBreedsVM> Handle(GetAllDogBreedsQuery request, CancellationToken cancellationToken)
    {
        var dbRecords = await _dogBreedRepository.GetAllAsync();

        return new DogBreedsVM
        {
            DogBreeds = _mapper.Map<List<DogBreedDto>>(dbRecords)
        };
    }
}