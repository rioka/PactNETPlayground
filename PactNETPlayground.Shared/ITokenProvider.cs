namespace PactNETPlayground.Shared;

public interface ITokenProvider {

    Task<string> GetToken(string userId, string audience = "producer-api", IDictionary<string, string>? customClaims = null);
}