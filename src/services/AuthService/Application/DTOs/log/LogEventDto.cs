namespace AuthService.Application.DTOs.log
{
    public class LogEventDto
    {
        public string UserId { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public string ActionStatus { get; set; } = string.Empty;
        public DateTime ActionTime { get; set; }
    }
}
