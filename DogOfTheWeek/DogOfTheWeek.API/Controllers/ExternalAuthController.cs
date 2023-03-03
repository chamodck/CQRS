using DogOfTheWeek.API.Services;
using DogOfTheWeek.Application.Common.Interfaces;
using DogOfTheWeek.Application.Common.Models;
using DogOfTheWeek.Application.Handlers.Identity.Models.IdentityDtos;
using DogOfTheWeek.Domain.Entities.ApplicationUserAggregate;
using DogOfTheWeek.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DogOfTheWeek.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExternalAuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;
    private readonly AppData _appData;
    private readonly IEmailHelper _emailHelper;
    private readonly JwtHandler _jwtHandler;
    public ExternalAuthController(ILogger<AuthController> logger, IMediator mediator, UserManager<ApplicationUser> userManager
        , RoleManager<IdentityRole> roleManager, IConfiguration configuration, IOptions<AppData> options
        , IEmailHelper emailHelper, JwtHandler jwtHandler)
    {
        this._userManager = userManager;
        this.roleManager = roleManager;
        this._configuration = configuration;
        this._logger = logger;
        this._mediator = mediator;
        this._appData = options.Value;
        this._emailHelper = emailHelper;
        this._jwtHandler = jwtHandler;
    }

    [HttpPost]
    [Route("GoogleLogin")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto externalAuth)
    {
        var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
        if (payload == null)
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(400), "Invalid External Authentication."));
        var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
        }
        if (user == null)
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(400), "User does not exists!"));
        else
            await _userManager.AddLoginAsync(user, info);

        //check for the Locked out account
        var token = await _jwtHandler.GenerateToken(user);
        return Ok(new AuthResponseDto { Token = token });
    }

    [HttpPost]
    [Route("GoogleRegister")]
    public async Task<IActionResult> GoogleRegister([FromBody] GoogleRegisterDto model)
    {
        var payload = await _jwtHandler.VerifyGoogleToken(new GoogleLoginDto { IdToken = model.IdToken, Provider = model.Provider });
        if (payload == null)
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(400), "Invalid External Authentication."));
        var info = new UserLoginInfo(model.Provider, payload.Subject, model.Provider);
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
        }
        if (user == null)
        {
            user = new ApplicationUser { Email = payload.Email, UserName = payload.Email, EmailConfirmed = true };
            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, model.UserRole.GetDescription());
            await _userManager.AddLoginAsync(user, info);
            return Ok(new AuthResponseDto { Token = await _jwtHandler.GenerateToken(user) });
        }
        else
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(400), "User already exists.Please login to continue."));
        }
    }
}