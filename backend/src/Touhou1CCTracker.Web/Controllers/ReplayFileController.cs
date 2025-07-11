using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Touhou1CCTracker.Application.DTOs.ReplayFile;
using Touhou1CCTracker.Application.Interfaces;

namespace Touhou1CCTracker.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ReplayFileController(IReplayFileService replayFileService) : ControllerBase
{
    [HttpGet("download/{id:long}")]
    [SwaggerOperation(
        Summary = "Download replay file by provided record ID",
        Description = "This method is used to download the replay file. Note: You must provide a record ID, not file ID!"
        )]
    public async Task<ActionResult> GetReplayFileByRecordIdAsync(long id)
    {
        try
        {
            return await replayFileService.GetDownloadLinkByRecordIdAsync(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("upload"), DisableRequestSizeLimit]
    [SwaggerOperation(
        Summary = "Upload replay file by provided record ID",
        Description = "This method uploads replay file by provided record ID. Note: You can't upload file if it's already exists!"
        )]
    public async Task<ActionResult> UploadReplayFile([FromForm] ReplayFileUploadDto uploadFileDto)
    {
        try
        {
            await replayFileService.UploadReplayFileAsync(uploadFileDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete/{id:long}")]
    [SwaggerOperation(
        Summary = "Delete replay file by provided record ID",
        Description = "This method deletes file by provided record ID. Note: You must provide a record ID, not file ID!"
        )]
    public async Task<ActionResult> DeleteReplayFileByRecordIdAsync(long id)
    {
        try
        {
            await replayFileService.DeleteReplayFileByRecordIdAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}