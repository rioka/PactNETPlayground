namespace PactNETPlayground.Producer.Features.Estimates.Models.Output; 

public class Estimate {

    public int Id { get; set; }
    
    public DateTime Date { get; set; }
    
    public string CustomerId { get; set; } = null!;

    public string MediaType { get; set; } = null!;
    
    public string? Contact { get; set; }
}
