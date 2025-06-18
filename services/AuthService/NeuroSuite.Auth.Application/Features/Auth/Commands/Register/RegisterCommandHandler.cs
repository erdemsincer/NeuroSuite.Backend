using MediatR;
using Microsoft.Extensions.Logging;
using NeuroSuite.Auth.Application.Common;
using NeuroSuite.Auth.Domain.Entities;
using NeuroSuite.Auth.Domain.Interfaces;
using NeuroSuite.BuildingBlocks.Contracts.Auth;

namespace NeuroSuite.Auth.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IRabbitMQPublisher _eventPublisher;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IPasswordHasher passwordHasher,
        ILogger<RegisterCommandHandler> logger,
        IRabbitMQPublisher eventPublisher)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _eventPublisher = eventPublisher;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser is not null)
            throw new Exception("Bu e-posta zaten kayıtlı.");

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = "User"
        };

        await _userRepository.AddAsync(user);

        // 🔊 Event yayınla
        _eventPublisher.PublishUserCreated(new UserCreatedEvent
        {
            Id = user.Id,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role
        });

        var token = _jwtProvider.GenerateToken(user);

        return new AuthenticationResult
        {
            Token = token,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}"
        };
    }
}
