using AuditService.Infrastructure.Messaging;

namespace AuditService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMQConsumer _consumer;

        public Worker(ILogger<Worker> logger, RabbitMQConsumer rabbitMQConsumer)
        {
            _logger = logger;
            _consumer = rabbitMQConsumer;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Worker] Démarrage PaymentService...");
            await _consumer.StartAsync(cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[Worker] En écoute sur RabbitMQ...");
            await _consumer.ConsumeAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _consumer.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
