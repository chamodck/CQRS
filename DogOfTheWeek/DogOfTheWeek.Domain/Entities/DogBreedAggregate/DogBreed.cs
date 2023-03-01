using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Domain.Entities.DogBreedAggregate;

public class DogBreed : BaseEntity<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
}