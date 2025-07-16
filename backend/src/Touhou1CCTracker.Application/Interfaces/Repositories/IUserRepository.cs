using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<bool> IsExistByUsernameAsync(string username);
    Task<bool> IsExistByIdAsync(long id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task AddUserAsync(User user);
    void DeleteUserAsync(User user);
    Task<int> SaveChangesAsync();
}