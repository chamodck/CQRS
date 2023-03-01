using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.Identity.Models.IdentityDtos
{
    public class UserDetailsResponse
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
