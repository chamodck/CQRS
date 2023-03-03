using DogOfTheWeek.Application.Common.Mappings;
using DogOfTheWeek.Domain.Entities.ApplicationUserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.Identity.Models.IdentityDtos;

public class ApplicationUserDto:IMapFrom<ApplicationUser>
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
public class GetAllApplicationUsersResponse : ApplicationUserDto
{
    public string Roles { get; set; }
}
public class ApplicationUserResponse : ApplicationUserDto
{
    public List<string> Roles { get; set; }
}
public class ApplicationUserUpdateRequest : ApplicationUserDto
{
}