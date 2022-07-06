using PactNETPlayground.Producer.Tests.Support;
using PactNETPlayground.Shared;

namespace PactNETPlayground.Producer.Tests;

/// <summary>
/// Startup class for tests
/// </summary>
internal class TestStartup {

    private readonly Startup _startup;

    public TestStartup(IConfiguration configuration) {

        _startup = new Startup();
    }
    
    public void ConfigureServices(IServiceCollection services) {

        // used to generate a valid token when a fake token is found in the request
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddTransient<BearerTokenReplacementMiddleware>();
        _startup.ConfigureServices(services);
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

        app.UseMiddleware<ProviderStateMiddleware>();
        app.UseMiddleware<BearerTokenReplacementMiddleware>();
        
        _startup.Configure(app, env);
    }
}