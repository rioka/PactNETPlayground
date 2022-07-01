using Microsoft.AspNetCore.Mvc;
using PactNETPlayground.Producer.Features.Estimates.Models.Input;
using PactNETPlayground.Producer.Features.Estimates.Models.Output;

namespace PactNETPlayground.Producer.Features.Estimates; 

[ApiController]
[Route("[controller]")]
public class EstimatesController : ControllerBase {

    internal const string GetByIdRoute = "GetById";
    
    [HttpGet("{id:int}", Name = GetByIdRoute)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<Estimate>> Get(int id) {
    
        return Task.FromResult((ActionResult<Estimate>) Ok(new Estimate()));
    }
    
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult> Create(CreateEstimate model) {
    
        var location = new Uri(Url.Link(GetByIdRoute, new { id = DateTime.Now.Millisecond })!);
        return Task.FromResult((ActionResult) Created(location, null));
    }
}