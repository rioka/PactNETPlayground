using PactNETPlayground.Producer.Domain;

namespace PactNETPlayground.Producer.Features.Estimates.Models.Input; 

public class CreateEstimate {

    public string CustomerId { get; set; } = null!;

    public MediaType MediaType { get; set; } 
}