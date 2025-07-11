using Touhou1CCTracker.Application.DTOs.ShotType;

namespace Touhou1CCTracker.Application.Interfaces;

public interface IShotTypeService
{
    Task<ShotTypeResponseDto> CreateShotTypeAsync(ShotTypeCreateOrUpdateDto requestDto);
    Task<ShotTypeResponseDto> GetShotTypeByIdAsync(long id);
    Task<IEnumerable<ShotTypeResponseDto>> GetAllShotTypesByCharacterNameAsync(string characterName);
    Task<IEnumerable<ShotTypeResponseDto>> GetAllShotTypesAsync();
    Task<ShotTypeResponseDto> UpdateShotTypeAsync(long id, ShotTypeCreateOrUpdateDto requestDto);
    Task DeleteShotTypeAsync(long id);
}