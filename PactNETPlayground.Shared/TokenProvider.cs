using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace PactNETPlayground.Shared; 

public class TokenProvider : ITokenProvider {

    public Task<string> GetToken(string userId, string audience = "producer-api", IDictionary<string, string>? customClaims = null) {
        
        var claims = new List<Claim> {
            new (ClaimTypes.Name, userId)
        };

        if (customClaims != null) {

            claims.AddRange(customClaims.Select(kvp => new Claim(kvp.Key, kvp.Value)));
        }
        
        var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = SecuritySettings.Issuer,
            Audience = audience,
            Subject = identity,
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(SecuritySettings.IssuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return Task.FromResult(tokenHandler.WriteToken(token));
    } 
}