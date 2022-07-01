namespace PactNETPlayground.Producer.Features.Estimates.Models.Output; 

public class Estimate {

    public DateTime Date { get; set; }
    
    public string CustomerId { get; set; }

    public string MediaType { get; set; }
}