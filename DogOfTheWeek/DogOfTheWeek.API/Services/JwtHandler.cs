using DogOfTheWeek.Application.Common.Interfaces;
using DogOfTheWeek.Application.Handlers.Identity.Models.IdentityDtos;
using DogOfTheWeek.Domain.Entities.ApplicationUserAggregate;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DogOfTheWeek.API.Services;

public class JwtHandler
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtSettings;
    private readonly IConfigurationSection _goolgeSettings;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtHandler(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("JWT");
        _goolgeSettings = _configuration.GetSection("GoogleAuthSettings");
        _userManager = userManager;
    }

    public async Task<string> GenerateToken(ApplicationUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(GoogleLoginDto externalAuth)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _goolgeSettings.GetSection("ClientId").Value }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);

            return payload;
        }
        catch (Exception ex)
        {
            //log an exception
            return null;
        }
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("Secret").Value);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings["ValidIssuer"],
            audience: _jwtSettings["ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["ExpiryInMinutes"])),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }
}