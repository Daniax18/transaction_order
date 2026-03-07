using AuthService.Application.DTOs.log;
using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Messaging;

namespace AuthService.Infrastructure.Services
{
    public class LogService : ILogService
    {

        private readonly RabbitMQPublisher _publisher;

        public LogService(RabbitMQPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task LogInfo(LogEventDto logEventDto)
        {
            await _publisher.PublishUserEventAsync(logEventDto);
        }
    }
}
