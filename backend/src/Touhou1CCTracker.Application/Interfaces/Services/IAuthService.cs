using Touhou1CCTracker.Application.DTOs.Login;

namespace Touhou1CCTracker.Application.Interfaces.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto requestDto);
    Task<Tuple<LoginResponseDto, string>> LoginAsync(LoginRequestDto requestDto);
}