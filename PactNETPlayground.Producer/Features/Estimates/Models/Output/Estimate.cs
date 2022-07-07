using PactNETPlayground.Producer.Domain;

namespace PactNETPlayground.Producer.Features.Estimates.Models.Output; 

public class Estimate {

    public int Id { get; set; }
    
    public DateTime Date { get; set; }
    
    public string CustomerId { get; set; } = null!;

    public MediaType MediaType { get; set; }
}