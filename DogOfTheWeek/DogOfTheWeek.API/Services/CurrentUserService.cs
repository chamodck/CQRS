using DogOfTheWeek.Application.Common.Interfaces;
using DogOfTheWeek.Domain.Entities.ApplicationUserAggregate;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DogOfTheWeek.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(
         IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    public string? Email
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var email = httpContext.User.FindFirst(ClaimTypes.Email).Value;
                return email;
            }
            else
            {
                return null;
            }
        }
        
    }
}
