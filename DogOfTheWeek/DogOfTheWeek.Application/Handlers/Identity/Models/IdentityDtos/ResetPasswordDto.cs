using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.Identity.Models.IdentityDtos
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
