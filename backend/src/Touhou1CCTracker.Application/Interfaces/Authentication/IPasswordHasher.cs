namespace Touhou1CCTracker.Application.Interfaces.Authentication;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string password, string hashedPassword);
}