using TransactionService.Application.Dto.Log;
using TransactionService.Application.Port.Outbound;

namespace TransactionService.Infrastructure.Adapter.Outbound.Messaging
{
    public class LogTransactionService : ILogTransactionService
    {
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public LogTransactionService(RabbitMQPublisher rabbitMQPublisher)
        {
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async Task LogTransactionAsync(
            string actionName, 
            bool isSuccess, 
            string userName, 
            string? errorMessage = null
        )
        {
            var logEventDto = new LogEventDto
            {
                UserId = userName,
                ActionName = actionName,
                ActionStatus = isSuccess ? "SUCCESS" : "FAILED" + errorMessage,
            };
            await _rabbitMQPublisher.PublisTransactionEventAsync(logEventDto);
        }
    }
}
