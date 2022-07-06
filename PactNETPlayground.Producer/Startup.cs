using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace PactNETPlayground.Producer;

public class Startup {

    public static readonly SymmetricSecurityKey IssuerSigningKey = new(Encoding.UTF8.GetBytes("zoff-gentile-cabrini"));
    
    public void ConfigureServices(IServiceCollection services) {

        services
            .AddHttpContextAccessor()
            .AddControllers();

        ConfigureAuthentication(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

        if (env.IsDevelopment()) {
            
            IdentityModelEventSource.ShowPII = true;
        }
        
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints => {

            endpoints.MapControllers();
        });
    }

    internal virtual void ConfigureAuthentication(IServiceCollection services) {
        
        services
            // reject anonymous requests by default 
            .AddAuthorization(config => {
                config.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();            
            })
            .AddAuthentication(options => {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {

                options.Authority = Shared.SecuritySettings.Issuer;
                options.Audience = "producer-api";
                options.TokenValidationParameters = new TokenValidationParameters() {
                    IssuerSigningKey = IssuerSigningKey
                };

                // prevent the OIDC metadata lookup since this isn't a real ID provider
                options.Configuration = new OpenIdConnectConfiguration {
                    Issuer = Shared.SecuritySettings.Issuer
                };
                options.RequireHttpsMetadata = false;
            });
    }
}