﻿using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace SolarWatch.Services.Authentication;

public class TokenService : ITokenService
{
    private const int ExpirationMinutes = 30;
    
    public string CreateToken(IdentityUser user)
    {
        var expirateion = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtToken(CreateClaims(user), CreateSigningCredentials(), expirateion);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) =>
        new(
            issuer: "apiWithAuthBackend",
            audience: "apiWithAuthBackend",
            claims, 
            expires: expiration,
            signingCredentials: credentials
        );

    private List<Claim> CreateClaims(IdentityUser user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };
            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!SomethingSecret!")),
        SecurityAlgorithms.HmacSha256);
    }
}