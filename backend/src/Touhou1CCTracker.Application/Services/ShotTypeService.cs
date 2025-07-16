using FluentValidation;
using Touhou1CCTracker.Application.DTOs.ShotType;
using Touhou1CCTracker.Application.Interfaces;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Application.Interfaces.Services;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Services;

public class ShotTypeService(IShotTypeRepository shotTypeRepository, IValidator<ShotTypeCreateOrUpdateDto> validator) : IShotTypeService
{
    public async Task<ShotTypeResponseDto> CreateShotTypeAsync(ShotTypeCreateOrUpdateDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (await shotTypeRepository.IsExistByNameAsync(requestDto.CharacterName, requestDto.ShotName))
            throw new Exception($"Character \"{requestDto.CharacterName}\" with shot type \"{requestDto.ShotName}\" already exists!");

        var shotType = new ShotType
        {
            CharacterName = requestDto.CharacterName.Trim(),
            ShotName = requestDto.ShotName.Trim()
        };
        
        await shotTypeRepository.AddShotTypeAsync(shotType);
        await shotTypeRepository.SaveChangesAsync();
        
        return MapToResponseDto(shotType);
    }

    public async Task<ShotTypeResponseDto> GetShotTypeByIdAsync(long id)
    {
        var shotType = await shotTypeRepository.GetShotTypeByIdAsync(id);
        if (shotType == null)
            throw new Exception($"Shot type with id \"{id}\" does not exist!");
        
        return MapToResponseDto(shotType);
    }

    public async Task<IEnumerable<ShotTypeResponseDto>> GetAllShotTypesByCharacterNameAsync(string characterName)
    {
        var shotTypes = await shotTypeRepository.GetAllShotTypesByCharacterNameAsync(characterName);
        if (shotTypes == null)
            throw new Exception($"Character \"{characterName}\" does not exist!");
        
        return shotTypes.Select(MapToResponseDto).ToList();
    }

    public async Task<IEnumerable<ShotTypeResponseDto>> GetAllShotTypesAsync()
    {
        var shotTypes = await shotTypeRepository.GetAllShotTypesAsync();
        if (shotTypes == null)
            throw new Exception($"No shot types exist in database!");
        
        return shotTypes.Select(MapToResponseDto).ToList();
    }

    public async Task<ShotTypeResponseDto> UpdateShotTypeAsync(long id, ShotTypeCreateOrUpdateDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var shotType = await shotTypeRepository.GetShotTypeByIdAsync(id);
        if (shotType == null)
            throw new Exception($"Shot type with id \"{id}\" does not exist!");

        if (await shotTypeRepository.IsExistByNameAsync(requestDto.CharacterName, requestDto.ShotName))
            throw new Exception($"Shot type \"{requestDto.CharacterName} {requestDto.ShotName}\" already exists!");
        
        shotType.CharacterName = requestDto.CharacterName.Trim();
        shotType.ShotName = requestDto.ShotName.Trim();
        
        await shotTypeRepository.SaveChangesAsync();
        
        return MapToResponseDto(shotType);
    }

    public async Task DeleteShotTypeAsync(long id)
    {
        var shotType = await shotTypeRepository.GetShotTypeByIdAsync(id);
        if (shotType == null)
            throw new Exception($"Shot type with id \"{id}\" does not exist!");
        
        if (shotType.Records.Any())
            throw new Exception($"There are records for shot type with id \"{id}\" in database!");
        
        shotTypeRepository.DeleteShotType(shotType);
        
        await shotTypeRepository.SaveChangesAsync();
    }

    private ShotTypeResponseDto MapToResponseDto(ShotType shotType)
    {
        return new ShotTypeResponseDto
        {
            Id = shotType.Id,
            CharacterName = shotType.CharacterName,
            ShotName = shotType.ShotName
        };
    }
}