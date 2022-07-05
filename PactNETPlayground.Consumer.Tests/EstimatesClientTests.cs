using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using PactNet;
using PactNet.Matchers;

namespace PactNETPlayground.Consumer.Tests; 

[TestFixture]
public class GetEstimateTests {

    private IPactBuilderV3 _builder = null!;

    [SetUp]
    public void BeforeEach() {
        
        var config = new PactConfig {
            // where we want to save generated files to be shared with the "producer" 
            PactDir = @"../../../../pacts/",
            LogLevel = PactLogLevel.Trace,
            DefaultJsonSettings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        };

        _builder = Pact
            // these two values here are used to set the name of the generated file 
            .V3("PactNETPlayground.Consumer", "PactNETPlayground.Provider", config)
            .UsingNativeBackend();
    }
    
    [Test]
    [TestCase(54)]
    public async Task GetEstimateTest(int id) {
        
        // arrange
        // this is our mock response
        var estimate = new Models.Estimate() {
            Id = 54,
            CustomerId = "Sample customer",
            MediaType = "Digital"
        };
        
        // define the interaction
        _builder
            .UponReceiving("A request to retrieve an estimate")
            .Given($"estimate with Id {id} exists")
            .WithRequest(HttpMethod.Get, $"/estimates/{id}")
            .WillRespond()
            .WithStatus(HttpStatusCode.OK)
            // TODO read docs: what changes if we use matchers? 
            .WithJsonBody(new {
                Id = 54,
                CustomerId = Match.Type("Sample customer"),
                MediaType =  Match.Type("Digital")
            });

        // assert
        await _builder
            .VerifyAsync(async ctx => {
                
                // now we generate the client
                // it is generated here because the docs clearly states to use "ctx.MockServerUri"
                // (I guess because it's dynamically generated)
                var client = new EstimatesClient(new HttpClient() {
                    BaseAddress = ctx.MockServerUri
                });
                
                var response = await client.GetEstimate(id);
                
                Assert.AreEqual(estimate.Id, response.Id);
                Assert.AreEqual(estimate.CustomerId, response.CustomerId);
                Assert.AreEqual(estimate.MediaType, response.MediaType);
        });
    }

    [Test]
    public async Task CreateEstimateTest() {
        
        // arrange
        // this is our mock response
        var data = new {
            Id = 54
        };
        var payload = new Models.CreateEstimate {
            CustomerId = "Sample Customer",
            MediaType = "Digital"
        };

        // define the interaction
        _builder
            .UponReceiving("A request to create an estimate")
                .Given("payload is valid")
                .WithRequest(HttpMethod.Post, "/estimates")
                .WithJsonBody(payload)
            .WillRespond()
                .WithStatus(HttpStatusCode.Created)
                .WithJsonBody(new TypeMatcher(data));
      
        // assert
        await _builder
            .VerifyAsync(async ctx => {
                
                var client = new EstimatesClient(new HttpClient() {
                    BaseAddress = ctx.MockServerUri
                });
                
                var estimateId = await client.CreateEstimate(payload);
                
                Assert.AreEqual(data.Id, estimateId);
            });
    }
}