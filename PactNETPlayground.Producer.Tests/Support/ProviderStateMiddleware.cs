using System.Net;
using System.Text;
using System.Text.Json;
using PactNet;

namespace PactNETPlayground.Producer.Tests.Support;

/// <summary>
/// Custom middleware, used by pact to set up our environment so that requests can receive the expected response.
/// For example, when a POST request is processed and a new item should be created, we can check our database
/// and verify we do not have a conflicting item already store, and if this is the case, we can delete it. 
/// </summary>
internal class ProviderStateMiddleware {

    private static readonly JsonSerializerOptions Options = new() {

        PropertyNameCaseInsensitive = true
    };

    private readonly IDictionary<string, Action<IDictionary<string, string>>> _providerStates;
    private readonly RequestDelegate _next;

    public ProviderStateMiddleware(RequestDelegate next) {
        
        _next = next;

        // Here we define some delegates that would be used when we need to 
        // prepare our service so that we may provide the expected response for each request
        // Keys in this dictionary are the values set in tests in `IRequestBuilderV3.Given`,
        // Note: currently doing nothing as there is no actual persistence
        _providerStates = new Dictionary<string, Action<IDictionary<string, string>>>() {
            ["estimate with Id 54 exists"] =  InsertEstimateIntoDatabase,
            ["payload is valid"] = NoOp,
            ["there are 5 estimates"] = NoOp
        };
    }
    
    public async Task InvokeAsync(HttpContext context) {
        
        // If this is a "real" request, just move on.
        if (!(context.Request.Path.Value?.StartsWith("/provider-states") ?? false)) {
            await _next.Invoke(context);

            return;
        }

        // At this point, it means we're processing a request to restore a precise state
        // captured as part of a test run by a client
        context.Response.StatusCode = (int) HttpStatusCode.OK;

        // context.Request.Body != null is redundant
        if (context.Request.Method == HttpMethod.Post.ToString() /*&& context.Request.Body != null*/) {
            
            string jsonRequestBody;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8)) {
            
                jsonRequestBody = await reader.ReadToEndAsync();
            }

            var providerState = JsonSerializer.Deserialize<ProviderState>(jsonRequestBody, Options);

            // A null or empty provider state key must be handled
            if (!string.IsNullOrEmpty(providerState?.State)) {
                
                // Get the delegate registered for a given state, and execute it.
                // This will in general perform some action so that we can set up a proper
                // "environment" and process the request correctly.
                _providerStates[providerState.State].Invoke(providerState.Params);
            }

            await context.Response.WriteAsync(string.Empty);
        }
    }

    #region Internals

    private void InsertEstimateIntoDatabase(IDictionary<string, string> parameters) {
        
        // Just a placeholder for now, as the code in the service has no persistence, 
        // so we do not need custom actions to set proper state when the request is processed
        // Leaving this method here as we may enhance this sample and handle more concrete scenarios soon
    }

    private void NoOp(IDictionary<string, string> parameters) {
    }

    #endregion
}