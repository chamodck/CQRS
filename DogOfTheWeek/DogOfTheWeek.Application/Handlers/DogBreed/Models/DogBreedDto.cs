using DogOfTheWeek.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.DogBreed.Models;

public class DogBreedDto : BaseVM, IMapFrom<DogOfTheWeek.Domain.Entities.DogBreedAggregate.DogBreed>
{
    public string Name { get; set; }
    public string Description { get; set; }
}
public class DogBreedRequestDto : DogBreedDto
{
    [JsonIgnore]
    public override DateTime CreatedOn { get; set; }
    [JsonIgnore]
    public override string? CreatedBy { get; set; }
    [JsonIgnore]
    public override DateTime? UpdatedOn { get; set; }
    [JsonIgnore]
    public override string? UpdatedBy { get; set; }
}