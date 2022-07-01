using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using PactNet;
using PactNet.Matchers;

namespace PactNETPlayground.Consumer.Tests; 

[TestFixture]
public class GetEstimateTests {

    private IPactBuilderV3 _builder;

    [SetUp]
    public void BeforeEach() {
        
        var config = new PactConfig {
            PactDir = @"../../../../pacts/",
            LogLevel = PactLogLevel.Trace,
            DefaultJsonSettings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        };

        _builder = Pact
            .V3("PactNETPlayground.Consumer", "PactNETPlayground.Provider", config)
            .UsingNativeBackend();    
    }
    
    [Test]
    [TestCase(54)]
    public async Task GetEstimateTest(int id) {
        
        // arrange
        var estimate = new Models.Estimate() {
            Id = 54,
            CustomerId = "Sample customer",
            MediaType = "Digital"
        };
        
        _builder
            .UponReceiving("A valid request for an estimate")
            .Given($"estimate with Id {id} exists")
            .WithRequest(HttpMethod.Get, $"/estimates/{id}")
            .WillRespond()
            .WithStatus(HttpStatusCode.OK)
            .WithJsonBody(new TypeMatcher(estimate));

        // assert
        await _builder
            .VerifyAsync(async ctx => {
                
                var client = new EstimatesClient(new HttpClient() {
                    BaseAddress = ctx.MockServerUri
                });
                
                var response = await client.GetEstimate(id);
                
                Assert.AreEqual(estimate.Id, response.Id);
                Assert.AreEqual(estimate.CustomerId, response.CustomerId);
                Assert.AreEqual(estimate.MediaType, response.MediaType);
        });
    }
}