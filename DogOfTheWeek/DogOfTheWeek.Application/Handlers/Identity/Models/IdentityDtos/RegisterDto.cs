using DogOfTheWeek.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Handlers.Identity.Models.IdentityDtos
{
    public class RegisterDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public UserRegisterRoleEnum UserRole { get; set; }
    }
}
