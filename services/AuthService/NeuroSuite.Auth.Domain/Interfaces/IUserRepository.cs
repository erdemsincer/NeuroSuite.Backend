using NeuroSuite.Auth.Domain.Entities;

namespace NeuroSuite.Auth.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
}
