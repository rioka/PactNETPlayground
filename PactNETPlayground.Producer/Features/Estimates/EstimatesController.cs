﻿using Microsoft.AspNetCore.Mvc;
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

        return Task.FromResult((ActionResult<Estimate>) Ok(new Estimate() {
            Id = id,
            CustomerId = $"Customer-{id}",
            Date = DateTime.Now.AddDays(-((DateTime.Now.Second + 1) % 5)).Date,
            MediaType = "Digital"
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
}