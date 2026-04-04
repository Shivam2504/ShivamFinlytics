namespace ShivamFinlytics.Application.DTOs;

public class ActivityLogDto
{
    public int ActivityId { get; set; }
    public string? UserName { get; set; }
    public string? Action { get; set; }
    public string? EntityName { get; set; }
    public int EntityId { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? Details { get; set; }
}