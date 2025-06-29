﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using NeuroSuite.Auth.Application.Features.Auth.Commands.Register;
using NeuroSuite.Auth.Application.Features.Auth.Queries.Login;

namespace NeuroSuite.Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
