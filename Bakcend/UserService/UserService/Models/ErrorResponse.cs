namespace UserService.Models;

public class ErrorResponse
{
    public string RequestId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    
    public ErrorResponse(string requestId, string message)
    {
        RequestId = requestId;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}
