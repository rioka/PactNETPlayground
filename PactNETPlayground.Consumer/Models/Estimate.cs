namespace PactNETPlayground.Consumer.Models;

internal class Estimate {

    public int Id { get; set; }
    
    public string CustomerId { get; set; } = null!;
    
    public string MediaType { get; set; } = null!;
}