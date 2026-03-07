using AuditService.Infrastructure.Messaging;

namespace AuditService
{
    // Worker hérite de BackgroundService.
    // BackgroundService permet de créer un service qui tourne en arrière-plan
    // (souvent utilisé dans les microservices ou workers .NET).
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        // Consumer RabbitMQ responsable de lire les messages dans la queue
        private readonly RabbitMQConsumer _consumer;

        public Worker(ILogger<Worker> logger, RabbitMQConsumer rabbitMQConsumer)
        {
            _logger = logger;
            _consumer = rabbitMQConsumer;
        }

        // Méthode appelée lorsque le service démarre
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Worker] Start Audit Service...");

            // Démarre le consumer RabbitMQ (connexion au broker, préparation de la queue)
            await _consumer.StartAsync(cancellationToken);

            // Appelle le démarrage du BackgroundService
            await base.StartAsync(cancellationToken);
        }

        // Méthode principale exécutée en arrière-plan
        // Elle tourne tant que le service est actif
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[Worker] Listenning RabbitMQ...");

            // Lance la consommation des messages de la queue
            // stoppingToken permet d'arrêter proprement le service
            await _consumer.ConsumeAsync(stoppingToken);
        }

        // Méthode appelée lorsque le service s'arrête
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Libère les ressources du consumer (connexion RabbitMQ, channels, etc.)
            await _consumer.DisposeAsync();

            // Arrêt du BackgroundService
            await base.StopAsync(cancellationToken);
        }
    }
}
