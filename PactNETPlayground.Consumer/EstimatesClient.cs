using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PactNETPlayground.Consumer.Models;

namespace PactNETPlayground.Consumer;

internal class EstimatesClient {
    
    private readonly HttpClient _client;
    
    public EstimatesClient(HttpClient client) {

        _client = client;
    }

    public async Task<Estimate> GetEstimate(int id, CancellationToken cancellationToken = default) {

        using (var response = await _client.GetAsync($"estimates/{id}", cancellationToken)) {

            if (response.IsSuccessStatusCode) {
                
                var result = await response.Content.ReadAsStringAsync(cancellationToken);

                return JsonConvert.DeserializeObject<Estimate>(result);
            }

            throw new Exception($"Request failed: {response.StatusCode}");
        }
    }

    public async Task<int> CreateEstimate(CreateEstimate model, CancellationToken cancellationToken = default) {
        
        using (var response = await _client.PostAsJsonAsync("estimates", model, cancellationToken)) {

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