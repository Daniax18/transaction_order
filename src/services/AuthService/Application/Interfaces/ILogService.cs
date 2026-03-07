using AuthService.Application.DTOs.log;

namespace AuthService.Application.Interfaces
{
    public interface ILogService
    {
        Task LogInfo(LogEventDto logEventDto);
    }
}
