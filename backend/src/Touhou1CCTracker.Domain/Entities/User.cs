namespace Touhou1CCTracker.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = "User";
}