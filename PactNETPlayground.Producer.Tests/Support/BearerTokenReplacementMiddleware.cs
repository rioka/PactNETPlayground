using PactNETPlayground.Shared;

namespace PactNETPlayground.Producer.Tests.Support;

/// <summary>
/// Process requests and replace a placeholder token with a valid one 
/// </summary>
internal class BearerTokenReplacementMiddleware : IMiddleware {
    
    private readonly ITokenProvider _tokenProvider;

    public BearerTokenReplacementMiddleware(ITokenProvider tokenProvider) {

        _tokenProvider = tokenProvider;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {

        if (context.Request.Headers.ContainsKey("Authorization")) {
            
            var token = await _tokenProvider.GetToken("Someone");
            context.Request.Headers["Authorization"] = $"Bearer {token}";
        }

        await next(context);
    }
}