namespace PactNETPlayground.Producer;

public class Startup {

    public void ConfigureServices(IServiceCollection services) {

        services
            .AddHttpContextAccessor()
            .AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        
        app.UseRouting();
        
        app.UseEndpoints(endpoints => {

            endpoints.MapControllers();
        });
    }
}