using NUnit.Framework;

namespace PactNETPlayground.Producer.Tests;

[SetUpFixture]
internal class ProducerTestSetup {
    
    private static IHost? _server;

    public static Uri ServerUri { get; } = BuildUri();

    [OneTimeSetUp]
    public void BeforeAny() {

        _server = Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder => {
                
                webBuilder.UseUrls(ServerUri.ToString());
                webBuilder.UseStartup<TestStartup>();
            })
            .Build();
        
        _server.Start();
    }

    [OneTimeTearDown]
    public void AfterAll() {
        
        _server?.Dispose();
    }
        
    #region Internals

    private static Uri BuildUri() {
        
        // TODO: Find an available port dynamically 
        return new Uri("http://localhost:9222");
    }

    #endregion
}