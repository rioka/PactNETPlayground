using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using PactNETPlayground.Producer.Features.Estimates.Models.Input;
using PactNETPlayground.Producer.Features.Estimates.Models.Output;

namespace PactNETPlayground.Producer.Features.Estimates; 

[ApiController]
[Route("[controller]")]
public class EstimatesController : ControllerBase {
    
    internal const string GetByIdRoute = "GetById";

    [HttpGet("{id:int}", Name = GetByIdRoute)]
    [ProducesResponseType(typeof(Estimate), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<Estimate>> Get(int id) {

        return Task.FromResult((ActionResult<Estimate>) Ok(new Estimate() {
            Id = id,
            CustomerId = $"Customer-{id}",
            Date = DateTime.Now.AddDays(-((DateTime.Now.Second + 1) % 5)).Date,
            MediaType = "Digital",
            Contact = $"someone-{id}@nowhere.org"
        }));
    }
    
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult> Create(CreateEstimate model) {

        var id = DateTime.Now.Millisecond;
        var location = new Uri(Url.Link(GetByIdRoute, new { id })!);
        return Task.FromResult((ActionResult) Created(location, new { id }));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<Estimate>), StatusCodes.Status200OK)]
    public Task<ActionResult<List<Estimate>>> Search() {

        var estimates = Enumerable.Range(1, 5)
            .Select(i => {
                
                var id = i * RandomNumberGenerator.GetInt32(1, 20_000);
                return new Estimate() {
                    Id = id,
                    CustomerId = $"Customer-{id}",
                    Date = DateTime.Now.AddDays(-((DateTime.Now.Second + 1) % 5)).Date,
                    MediaType = "Digital",
                    Contact = $"someone-{id}@nowhere.org"
                };
            });
        
        return Task.FromResult((ActionResult<List<Estimate>>) Ok(estimates));
    }
}