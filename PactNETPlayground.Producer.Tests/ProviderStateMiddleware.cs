using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace PactNETPlayground.Producer.Tests;

internal class ProviderStateMiddleware {

    private static readonly JsonSerializerOptions Options = new() {

        PropertyNameCaseInsensitive = true
    };

    private readonly IDictionary<string, Action<IDictionary<string, string>>> _providerStates;
    private readonly RequestDelegate _next;

    public ProviderStateMiddleware(RequestDelegate next) {
        
        _next = next;

        // keys are the values set in tests in `IRequestBuilderV3.Given`
        // currently doing nothing as there is no actual persistence
        _providerStates = new Dictionary<string, Action<IDictionary<string, string>>>() {
            ["estimate with Id 54 exists"] =  InsertEstimateIntoDatabase,
            ["payload is valid"] = NoOp
        };
    }

    private void InsertEstimateIntoDatabase(IDictionary<string, string> parameters) {
    }

    private void NoOp(IDictionary<string, string> parameters) {
    }
    
    public async Task InvokeAsync(HttpContext context) {
        
        if (!(context.Request.Path.Value?.StartsWith("/provider-states") ?? false)) {
            await _next.Invoke(context);

            return;
        }

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
                
                _providerStates[providerState.State].Invoke(providerState.Params);
            }

            await context.Response.WriteAsync(string.Empty);
        }
    }
}