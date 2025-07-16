using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Interfaces.Authentication;

public interface IJwtProvider
{
    string GenerateToken(User user);
}