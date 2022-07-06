using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PactNETPlayground.Shared; 

public class SecuritySettings {

    public static readonly SymmetricSecurityKey IssuerSigningKey = new(Encoding.UTF8.GetBytes("zoff-gentile-cabrini"));
    
    public static readonly string Issuer = "https://fake.onelogin.org";
}