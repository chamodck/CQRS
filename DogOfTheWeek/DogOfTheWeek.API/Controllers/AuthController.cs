using DogOfTheWeek.Application.Common.Interfaces;
using DogOfTheWeek.Application.Common.Models;
using DogOfTheWeek.Application.Handlers.Identity.Models.IdentityDtos;
using DogOfTheWeek.Domain.Entities.ApplicationUserAggregate;
using DogOfTheWeek.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DogOfTheWeek.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;
    private readonly AppData _appData;
    private readonly IEmailHelper _emailHelper;
    public AuthController(ILogger<AuthController> logger, IMediator mediator, UserManager<ApplicationUser> userManager
        , RoleManager<IdentityRole> roleManager, IConfiguration configuration, IOptions<AppData> options
        , IEmailHelper emailHelper)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this._configuration = configuration;
        this._logger = logger;
        this._mediator = mediator;
        this._appData = options.Value;
        this._emailHelper = emailHelper;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        {
            bool emailStatus = await userManager.IsEmailConfirmedAsync(user);
            if (!emailStatus)
                return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "Email is unconfirmed,Check your mail & confirm it first."));

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(401), "Email and Password does not match"));
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        try
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "User already exists!"));

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "User creation failed! Please check user details and try again."));

            if (await roleManager.RoleExistsAsync(model.UserRole.GetDescription()))
            {
                await userManager.AddToRoleAsync(user, model.UserRole.GetDescription());
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            string confirmationToken = HttpUtility.UrlEncode($"{token}&{user.Email}");
            string confirmationLink = $"{_appData.CLIENT_APP_URL}/confirm-email/{confirmationToken}";

            bool emailResponse = _emailHelper.SendEmailConfirm(user.Email, confirmationLink);

            if (emailResponse)
            {
                return Ok(new Response<string>("User created successfully.Check your email and confirm it."));
            }
            else
            {
                await userManager.ConfirmEmailAsync(user, token);
                return Ok(new Response<string>("User created successfully!"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), $"{ex.Message}"));
        }
    }

    [HttpPost]
    [Route("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "User not found"));

        var result = await userManager.ConfirmEmailAsync(user, model.Token);
        if (result.Succeeded)
        {
            return Ok(new Response<string>("User email successfully confirmed!"));
        }
        else
        {
            return Ok(new Response<string>("User email confirm was failed.Try again."));
        }
    }

    [HttpPost]
    [Route("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "User not found"));

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        string confirmationToken = HttpUtility.UrlEncode($"{token}&{user.Email}");
        string confirmationLink = $"{_appData.CLIENT_APP_URL}/reset-password/{confirmationToken}";

        bool emailResponse = _emailHelper.SendEmailPasswordReset(user.Email, confirmationLink);
        if (emailResponse)
        {
            return Ok(new Response<string>("Check your email to reset password"));
        }
        else
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "Password reset was failed, Try again."));
        }
    }
    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
    {

        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "User not found"));

        var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
        {
            return Ok(new Response<string>("Password has been reset."));
        }
        else
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "Password Reset was failed.Try again."));
        }
    }
    [HttpPost]
    [Route("login-admin")]
    public async Task<IActionResult> AdminLogin([FromBody] LoginDto model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        {
            bool emailStatus = await userManager.IsEmailConfirmedAsync(user);
            if (!emailStatus)
                return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "Email is unconfirmed, please confirm it first."));

            var userRoles = await userManager.GetRolesAsync(user);

            if (userRoles.Contains(UserRoles.Administrator))
            {
                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new Response<object>(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                }));
            }
            else
            {
                return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(401), "Email and Password does not match"));
            }
        }
        return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(401), "Email and Password does not match"));
    }

    [HttpPost]
    [Route("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterDto model)
    {
        var userExists = await userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "User already exists!"));

        ApplicationUser user = new ApplicationUser()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Email,
            EmailConfirmed = true
        };

        string randomPassword = GenerateRandomPassword(null);
        var result = await userManager.CreateAsync(user, randomPassword);
        if (!result.Succeeded)
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "User creation failed! Please check user details and try again."));

        if (!await roleManager.RoleExistsAsync(UserRoles.Administrator))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Administrator));
        if (!await roleManager.RoleExistsAsync(UserRoles.Administrator))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Administrator));

        if (await roleManager.RoleExistsAsync(UserRoles.Administrator))
        {
            await userManager.AddToRoleAsync(user, UserRoles.Administrator);
        }

        var dbUser = await userManager.FindByEmailAsync(model.Email);
        if (dbUser == null)
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "User not found"));

        var token = await userManager.GeneratePasswordResetTokenAsync(dbUser);
        string confirmationToken = HttpUtility.UrlEncode($"{token}&{user.Email}");
        string confirmationLink = $"{_appData.ADMIN_PORTAL_URL}/reset-password/{confirmationToken}";

        bool emailResponse = _emailHelper.SendEmailPasswordReset(user.Email, confirmationLink);
        if (emailResponse)
        {

            return Ok(new Response<string>("User created successfully!"));
        }
        else
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), "Password reset was failed, Try again."));
        }
    }

    public static string GenerateRandomPassword(PasswordOptions opts = null)
    {
        if (opts == null) opts = new PasswordOptions()
        {
            RequiredLength = 6,
            RequireDigit = true,
            RequireLowercase = true,
            RequireNonAlphanumeric = true,
            RequireUppercase = true
        };

        string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

        Random rand = new Random(Environment.TickCount);
        List<char> chars = new List<char>();

        if (opts.RequireUppercase)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[0][rand.Next(0, randomChars[0].Length)]);

        if (opts.RequireLowercase)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[1][rand.Next(0, randomChars[1].Length)]);

        if (opts.RequireDigit)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[2][rand.Next(0, randomChars[2].Length)]);

        if (opts.RequireNonAlphanumeric)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[3][rand.Next(0, randomChars[3].Length)]);

        for (int i = chars.Count; i < opts.RequiredLength
            || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
        {
            string rcs = randomChars[rand.Next(0, randomChars.Length)];
            chars.Insert(rand.Next(0, chars.Count),
                rcs[rand.Next(0, rcs.Length)]);
        }

        return new string(chars.ToArray());
    }
}