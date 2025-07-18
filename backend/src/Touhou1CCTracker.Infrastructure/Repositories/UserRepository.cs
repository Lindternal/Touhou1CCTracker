using Microsoft.EntityFrameworkCore;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Infrastructure.Data;

namespace Touhou1CCTracker.Infrastructure.Repositories;

public class UserRepository(Touhou1CCTrackerDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> IsExistByUsernameAsync(string username)
    {
        return await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == username);
    }

    public async Task<bool> IsExistByIdAsync(long id)
    {
        return await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.Users
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public void DeleteUserAsync(User user)
    {
        context.Users.Remove(user);
    }
    
    public async Task<int> SaveChangesAsync()
    { 
        return await context.SaveChangesAsync();
    }
}