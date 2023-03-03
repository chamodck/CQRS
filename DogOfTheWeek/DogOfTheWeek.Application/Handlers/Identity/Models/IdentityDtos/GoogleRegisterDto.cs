using DogOfTheWeek.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.Identity.Models.IdentityDtos;

public class GoogleRegisterDto
{
    public string Provider { get; set; }
    public string IdToken { get; set; }
    public UserRegisterRoleEnum UserRole { get; set; }
}
