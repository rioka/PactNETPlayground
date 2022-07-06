using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PactNETPlayground.Consumer.Models;
using PactNETPlayground.Shared;

namespace PactNETPlayground.Consumer;

internal class EstimatesClient {
    
    private readonly HttpClient _client;
    private readonly ITokenProvider _tokenProvider;

    public string? UserId { get; set; } 
    
    public EstimatesClient(HttpClient client, ITokenProvider tokenProvider) {

        _client = client;
        _tokenProvider = tokenProvider;
    }

    public async Task<Estimate> GetEstimate(int id, CancellationToken cancellationToken = default) {

        using (var message = new HttpRequestMessage(HttpMethod.Get, $"estimates/{id}")) {

            await AddAuthorization(message);
            
            using (var response = await _client.SendAsync(message, cancellationToken)) {

                if (response.IsSuccessStatusCode) {
                
                    var result = await response.Content.ReadAsStringAsync(cancellationToken);

                    return JsonConvert.DeserializeObject<Estimate>(result);
                }

                throw new Exception($"Request failed: {response.StatusCode}");
            }
        }
    }

    public async Task<int> CreateEstimate(CreateEstimate model, CancellationToken cancellationToken = default) {
        
        using (var message = new HttpRequestMessage(HttpMethod.Post, "estimates")) {

            await AddAuthorization(message);
            message.Content = JsonContent.Create(model);
                
            using (var response = await _client.SendAsync(message, cancellationToken)) {

                if (response.IsSuccessStatusCode) {
                
                    var result = await response.Content.ReadAsStringAsync(cancellationToken);
                    var payload = JObject.Parse(result);
                    var idObj = payload["id"];
                    var id = idObj?.Value<int>(); 
                        
                    if (id.HasValue) {
                    
                        return id.Value;
                    }

                    throw new Exception($"Unexpected payload: {payload}");
                }

                throw new Exception($"Request failed: {response.StatusCode}");
            }
        }
    }

    #region Internals

    private async Task AddAuthorization(HttpRequestMessage message) {
 
        if (!string.IsNullOrWhiteSpace(UserId)) {
                
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenProvider.GetToken(UserId));
        }
    }

    #endregion
}