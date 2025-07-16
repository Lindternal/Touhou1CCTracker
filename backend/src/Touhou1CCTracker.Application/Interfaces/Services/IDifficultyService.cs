using Touhou1CCTracker.Application.DTOs.Difficulty;

namespace Touhou1CCTracker.Application.Interfaces.Services;

public interface IDifficultyService
{
    Task<DifficultyResponseDto> CreateDifficultyAsync(DifficultyCreateOrUpdateDto requestDto);
    Task<DifficultyResponseDto> GetDifficultyByIdAsync(long id);
    Task<IEnumerable<DifficultyResponseDto>> GetAllDifficultiesAsync();

    Task<DifficultyResponseDto> UpdateDifficultyAsync(long id,
        DifficultyCreateOrUpdateDto difficultyCreateOrUpdateDto);

    Task DeleteDifficultyAsync(long id);
}