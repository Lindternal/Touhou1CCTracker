using FluentValidation;
using Touhou1CCTracker.Application.DTOs.Difficulty;
using Touhou1CCTracker.Application.Interfaces;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Application.Interfaces.Services;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Services;

public class DifficultyService(IDifficultyRepository difficultyRepository, IValidator<DifficultyCreateOrUpdateDto> validator) : IDifficultyService
{
    public async Task<DifficultyResponseDto> CreateDifficultyAsync(DifficultyCreateOrUpdateDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (await difficultyRepository.IsExistByNameAsync(requestDto.Name))
            throw new Exception($"The difficulty \"{requestDto.Name}\" already exists!");

        var difficulty = new Difficulty()
        {
            Name = requestDto.Name.Trim()
        };
        
        await difficultyRepository.AddDifficultyAsync(difficulty);
        await difficultyRepository.SaveChangesAsync();
        
        return MapToResponseDto(difficulty);
    }

    public async Task<DifficultyResponseDto> GetDifficultyByIdAsync(long id)
    {
        var difficulty = await difficultyRepository.GetDifficultyByIdAsync(id);
        if (difficulty == null)
            throw new Exception($"Difficulty with id \"{id}\" does not exist!");
        
        return MapToResponseDto(difficulty);
    }

    public async Task<IEnumerable<DifficultyResponseDto>> GetAllDifficultiesAsync()
    {
        var difficulties = await difficultyRepository.GetAllDifficultiesAsync();
        return difficulties.Select(MapToResponseDto).ToList();
    }

    public async Task<DifficultyResponseDto> UpdateDifficultyAsync(long id,
        DifficultyCreateOrUpdateDto difficultyCreateOrUpdateDto)
    {
        var validationResult = await validator.ValidateAsync(difficultyCreateOrUpdateDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var difficulty = await difficultyRepository.GetDifficultyByIdAsync(id);
        if (difficulty == null)
            throw new Exception($"Difficulty with id \"{id}\" doesn't exist!");
        
        if (await difficultyRepository.IsExistByNameAsync(difficultyCreateOrUpdateDto.Name))
            throw new Exception($"Difficulty with name \"{difficultyCreateOrUpdateDto.Name}\" already exists!");
        
        if (!string.IsNullOrEmpty(difficultyCreateOrUpdateDto.Name))
            difficulty.Name = difficultyCreateOrUpdateDto.Name.Trim();
        
        await difficultyRepository.SaveChangesAsync();
        
        return MapToResponseDto(difficulty);
    }

    public async Task DeleteDifficultyAsync(long id)
    {
        var difficulty = await difficultyRepository.GetDifficultyByIdAsync(id);
        if (difficulty == null)
            throw new Exception($"Difficulty with id \"{id}\" doesn't exist!");
        
        if (difficulty.Records.Any())
            throw new Exception($"Can not delete difficulty \"{difficulty.Name}\" because it has records!");
        
        difficultyRepository.DeleteDifficulty(difficulty);
        
        await difficultyRepository.SaveChangesAsync();
    }
    
    private DifficultyResponseDto MapToResponseDto(Difficulty difficulty)
    {
        return new DifficultyResponseDto()
        {
            Id = difficulty.Id,
            Name = difficulty.Name
        };
    }
}