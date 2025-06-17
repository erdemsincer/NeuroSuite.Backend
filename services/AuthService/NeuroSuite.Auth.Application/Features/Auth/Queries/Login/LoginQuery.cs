using MediatR;
using NeuroSuite.Auth.Application.Common;

namespace NeuroSuite.Auth.Application.Features.Auth.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
) : IRequest<AuthenticationResult>;
