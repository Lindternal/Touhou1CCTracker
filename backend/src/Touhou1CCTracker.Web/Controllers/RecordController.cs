using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Touhou1CCTracker.Application.DTOs.Record;
using Touhou1CCTracker.Application.Interfaces;
using Touhou1CCTracker.Application.Services;

namespace Touhou1CCTracker.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class RecordController(IRecordService recordService) : ControllerBase
{
    [HttpGet("{gameId:long}")]
    [SwaggerOperation(
        Summary = "Get records by game ID",
        Description = "Returns all records related to the game whose ID was provided."
        )]
    public async Task<ActionResult<RecordResponseDto>> GetRecordsByGameIdAsync(long gameId)
    {
        try
        {
            var records = await recordService.GetRecordsByGameIdAsync(gameId);
            return Ok(records);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("paged/{page:int},{pageSize:int}")]
    [SwaggerOperation(
        Summary = "Get page of records by provided page number",
        Description = "Returns page of records by provided page number. Default is the first page. Default page size is 20 records. Records sorted by date."
        )]
    public async Task<ActionResult<RecordPagedResponseDto>> GetPagedLatestRecordsAsync(int page = 1, int pageSize = 20)
    {
        try
        {
            var records = await recordService.GetPagedLatestRecordsAsync(page, pageSize);
            return Ok(records);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add new record",
        Description = "Creates new record. You must provide a valid game ID, difficulty ID, shot type ID, a rank and date!"
        )]
    public async Task<ActionResult<RecordResponseDto>> CreateRecord([FromBody] RecordCreateOrUpdateDto requestDto)
    {
        try
        {
            var record = await recordService.CreateRecordAsync(requestDto);
            return Ok(record);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:long}")]
    [SwaggerOperation(
        Summary = "Edit record by provided record ID",
        Description = "Edits record by provided record ID. You must provide a valid game ID, difficulty ID, shot type ID, a rank and date!"
        )]
    public async Task<ActionResult<RecordResponseDto>> UpdateRecord(long id,
        [FromBody] RecordCreateOrUpdateDto requestDto)
    {
        try
        {
            var record = await recordService.UpdateRecordAsync(id, requestDto);
            return Ok(record);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:long}")]
    [SwaggerOperation(
        Summary = "Delete record by provided record ID",
        Description = "Deletes record by provided record ID. Note: Replay file that linked to this record will be also deleted!"
    )]
    public async Task<ActionResult> DeleteRecord(long id)
    {
        try
        {
            await recordService.DeleteRecordAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}