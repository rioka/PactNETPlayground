using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        
        _startup.ConfigureServices(services);
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

        app.UseMiddleware<ProviderStateMiddleware>();
        _startup.Configure(app, env);
    }
}