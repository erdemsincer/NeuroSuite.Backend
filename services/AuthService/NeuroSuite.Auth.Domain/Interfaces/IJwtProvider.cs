using NeuroSuite.Auth.Domain.Entities;

namespace NeuroSuite.Auth.Domain.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
