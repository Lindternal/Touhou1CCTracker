using FluentValidation;
using Touhou1CCTracker.Application.DTOs.Login;
using Touhou1CCTracker.Application.Interfaces.Authentication;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Application.Interfaces.Services;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Services;

public class AuthService(IUserRepository userRepository,
    IValidator<LoginRequestDto> loginValidator,
    IValidator<RegisterDto> registerDtoValidator,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider) : IAuthService
{
    public async Task RegisterAsync(RegisterDto requestDto)
    {
        var validationResult = await registerDtoValidator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var existingUser = await userRepository.IsExistByUsernameAsync(requestDto.Username);
        if (existingUser) throw new Exception("User already exists!");

        var user = new User
        {
            Username = requestDto.Username,
            PasswordHash = passwordHasher.HashPassword(requestDto.Password)
        };
        
        await userRepository.AddUserAsync(user);
        await userRepository.SaveChangesAsync();
    }
    
    public async Task<Tuple<LoginResponseDto, string>> LoginAsync(LoginRequestDto requestDto)
    {
        var validationResult = await loginValidator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await userRepository.GetUserByUsernameAsync(requestDto.Username);
        if (user == null)
            throw new Exception("Invalid username or password!");

        if (!passwordHasher.VerifyHashedPassword(requestDto.Password, user.PasswordHash))
            throw new Exception("Invalid username or password!");

        var token = jwtProvider.GenerateToken(user);

        return Tuple.Create(new LoginResponseDto
        {
            UserName = user.Username,
            Role = user.Role,
        }, token);
    }
}