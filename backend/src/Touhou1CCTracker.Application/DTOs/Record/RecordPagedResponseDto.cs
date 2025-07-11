namespace Touhou1CCTracker.Application.DTOs.Record;

public class RecordPagedResponseDto
{
    public IEnumerable<RecordResponseDto> Records { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}