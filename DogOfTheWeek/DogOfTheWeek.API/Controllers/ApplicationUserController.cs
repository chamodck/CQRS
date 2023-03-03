using AutoMapper;
using DogOfTheWeek.Application.Common.Models;
using DogOfTheWeek.Application.Handlers.Identity.Models.IdentityDtos;
using DogOfTheWeek.Domain.Entities.ApplicationUserAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogOfTheWeek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public ApplicationUserController( UserManager<ApplicationUser> userManager
            , IMapper mapper)
        {
            this._userManager = userManager;
            this._mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [Route("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
                var dbUser = await _userManager.FindByEmailAsync(email);
                var roles = await _userManager.GetRolesAsync(dbUser);
                var user = _mapper.Map<ApplicationUserResponse>(dbUser);
                user.Roles = roles.ToList();
                return Ok(new Response<ApplicationUserResponse>(user));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), $"{ex.Message}"));
            }
        }
    }
}
