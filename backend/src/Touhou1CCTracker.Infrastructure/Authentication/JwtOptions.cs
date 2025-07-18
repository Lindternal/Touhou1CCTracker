using Touhou1CCTracker.Application.Interfaces.Authentication;

namespace Touhou1CCTracker.Infrastructure.Authentication;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; }
}