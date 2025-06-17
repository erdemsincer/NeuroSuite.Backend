using NeuroSuite.User.Domain.Entities;

namespace NeuroSuite.User.Domain.Interfaces;

public interface IUserProfileRepository
{
    Task AddAsync(UserProfile user);
    Task<UserProfile?> GetByIdAsync(Guid id);
    Task<UserProfile?> GetByEmailAsync(string email);
}
