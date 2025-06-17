using Microsoft.EntityFrameworkCore;
using NeuroSuite.User.Domain.Entities;
using NeuroSuite.User.Domain.Interfaces;
using NeuroSuite.User.Infrastructure.Persistence;

namespace NeuroSuite.User.Infrastructure.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly UserDbContext _context;

    public UserProfileRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserProfile user)
    {
        await _context.UserProfiles.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserProfile?> GetByIdAsync(Guid id)
    {
        return await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<UserProfile?> GetByEmailAsync(string email)
    {
        return await _context.UserProfiles.FirstOrDefaultAsync(x => x.Email == email);
    }
}
