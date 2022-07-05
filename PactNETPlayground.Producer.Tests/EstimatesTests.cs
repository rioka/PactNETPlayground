using NUnit.Framework;
using PactNet;
using PactNet.Verifier;

namespace PactNETPlayground.Producer.Tests; 

[TestFixture]
public class EstimatesTests {

    [Test]
    public void Pact_With_Consumer_Is_Honored() {

        // arrange
        var config = new PactVerifierConfig() {
            LogLevel = PactLogLevel.Trace
        };

        var path = @"../../../../pacts/";
        var verifier = new PactVerifier(config);

        // assert
        verifier
            .ServiceProvider("Estimates API", ProducerTestSetup.ServerUri)
            .WithDirectorySource(new DirectoryInfo(path))
            .WithProviderStateUrl(new Uri(ProducerTestSetup.ServerUri, "/provider-states"))
            //.WithRequestTimeout(TimeSpan.FromSeconds(2))
            // .WithSslVerificationDisabled()
            .Verify();
    }
}