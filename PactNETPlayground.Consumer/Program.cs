using Microsoft.Extensions.DependencyInjection;
using PactNETPlayground.Consumer.Models;
using PactNETPlayground.Shared;

namespace PactNETPlayground.Consumer {

    public static class Program {

        public static async Task Main(string[] args) {

            var serviceCollection = new ServiceCollection()
                .AddLogging();

            serviceCollection.AddScoped<ITokenProvider, TokenProvider>();
            
            serviceCollection
                .AddHttpClient<EstimatesClient>(c => {
                    c.BaseAddress = new Uri("https://localhost:7096/");
                });
            
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<EstimatesClient>();
            client.UserId = "AnyTown";

            var estimate = await client.GetEstimate(123);

            Console.WriteLine($"Estimate {estimate.Id}{Environment.NewLine}{estimate.CustomerId}");
            
            var estimateId = await client.CreateEstimate(new CreateEstimate() {
                CustomerId = "Sample customer",
                MediaType = "Digital"
            });

            Console.WriteLine($"New estimate created: {estimateId}");
        }
    }
}
