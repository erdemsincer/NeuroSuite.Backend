using MediatR;
using Microsoft.AspNet.Identity;
using NeuroSuite.Auth.Application.Common;
using NeuroSuite.Auth.Domain.Interfaces;
using IPasswordHasher = NeuroSuite.Auth.Domain.Interfaces.IPasswordHasher;

namespace NeuroSuite.Auth.Application.Features.Auth.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;

    public LoginQueryHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            throw new Exception("E-posta veya şifre hatalı.");

        var isValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isValid)
            throw new Exception("E-posta veya şifre hatalı.");

        var token = _jwtProvider.GenerateToken(user);

        return new AuthenticationResult
        {
            Token = token,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}"
        };
    }
}
