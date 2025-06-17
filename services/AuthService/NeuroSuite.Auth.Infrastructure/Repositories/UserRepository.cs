using Microsoft.EntityFrameworkCore;
using NeuroSuite.Auth.Domain.Entities;
using NeuroSuite.Auth.Domain.Interfaces;
using NeuroSuite.Auth.Infrastructure.Persistence;

namespace NeuroSuite.Auth.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
