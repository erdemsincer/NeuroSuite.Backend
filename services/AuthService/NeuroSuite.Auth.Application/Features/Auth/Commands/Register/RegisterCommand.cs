using MediatR;
using NeuroSuite.Auth.Application.Common;

namespace NeuroSuite.Auth.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<AuthenticationResult>;
