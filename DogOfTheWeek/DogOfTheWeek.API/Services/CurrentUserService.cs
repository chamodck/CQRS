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
                var claim = httpContext.User.FindFirst(ClaimTypes.Email);
                if (claim != null)
                    return claim.Value;
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        
    }
}
