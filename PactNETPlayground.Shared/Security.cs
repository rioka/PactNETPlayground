using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace PactNETPlayground.Shared; 

public class Security {

    public static readonly SymmetricSecurityKey IssuerSigningKey = new(Encoding.UTF8.GetBytes("zoff-gentile-cabrini"));
    public static readonly string Issuer = "https://fake.onelogin.org";
   
    public static string GetToken(string userId, string audience = "producer-api", IDictionary<string, string>? customClaims = null) {
        
        var claims = new List<Claim> {
            new (ClaimTypes.Name, userId)
        };

        if (customClaims != null) {

            claims.AddRange(customClaims.Select(kvp => new Claim(kvp.Key, kvp.Value)));
        }
        
        var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = Issuer,
            Audience = audience,
            Subject = identity,
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(IssuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}