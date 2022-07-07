namespace PactNETPlayground.Producer.Features.Estimates.Models.Input; 

public class CreateEstimate {

    public string CustomerId { get; set; } = null!;

    public string MediaType { get; set; } = null!;
    
    public string? Contact { get; set; }
}