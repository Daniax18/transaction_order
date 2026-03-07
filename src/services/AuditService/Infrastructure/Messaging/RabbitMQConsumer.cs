using AuditService.Application.UseCases;
using AuditService.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuditService.Infrastructure.Messaging
{
    // Cette classe est responsable de consommer les messages venant de RabbitMQ
    // et de déclencher le traitement du UseCase ProcessAuditUseCase.
    public class RabbitMQConsumer : IAsyncDisposable
    {
        private readonly ProcessAuditUseCase _processAuditUseCase;  // UseCase métier qui traite les événements d'audit
        private IConnection? _connection;                           // Connexion RabbitMQ
        private IChannel? _channel;                                 // Channel RabbitMQ (canal pour envoyer/recevoir les messages)
        private const string QueueName = "log.created";             // Nom de la queue RabbitMQ à écouter

        public RabbitMQConsumer(ProcessAuditUseCase processAuditUseCase)
        {
            _processAuditUseCase = processAuditUseCase;
        }

        // Méthode appelée au démarrage du service pour se connecter à RabbitMQ
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };


            _connection = await factory.CreateConnectionAsync(cancellationToken);                       // Création de la connexion au serveur RabbitMQ
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);      // Création d'un canal de communication

            // Déclaration de la queue si elle n'existe pas encore
            await _channel.QueueDeclareAsync(
                queue: QueueName, 
                durable: true,                                  // la queue survit à un redémarrage du broker
                exclusive: false,                               // plusieurs consommateurs peuvent l'utiliser
                autoDelete: false,                              // la queue n'est pas supprimée automatiquement
                cancellationToken: cancellationToken
            );

            // Configuration du QoS (Quality of Service)
            // prefetchCount = 1 signifie que RabbitMQ envoie 1 message à la fois (Même si il y a 100 dans la queue)
            await _channel.BasicQosAsync(
                 prefetchSize: 0,
                 prefetchCount: 1,
                 global: false,
                 cancellationToken: cancellationToken
            );
        }

        // Méthode principale qui écoute et consomme les messages
        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            // Création du consommateur asynchrone
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            // Cette méthode est appelée à chaque réception d'un message
            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonSerializer.Deserialize<AuditEvent>(json);

                    if (message != null)
                    {
                        await _processAuditUseCase.ExecuteAsync(
                            message.UserId,
                            message.ServiceName,
                            message.ActionName,
                            message.ActionStatus,
                            message.ActionTime
                        );
                    }

                    // Confirmation à RabbitMQ que le message a été traité avec succès
                    await _channel!.BasicAckAsync(args.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    // En cas d'erreur, on renvoie le message dans la queue (requeue)
                    await _channel!.BasicNackAsync(
                        deliveryTag: args.DeliveryTag,
                        multiple: false,
                        requeue: true
                    );
                }
            };

            // Démarrage de la consommation des messages
            await _channel!.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,                 // on confirme manuellement avec BasicAck
                consumer: consumer,
                cancellationToken: cancellationToken
            );

            // Le service reste actif indéfiniment
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        // Méthode appelée lors de l'arrêt du service pour fermer proprement la connexion
        public async ValueTask DisposeAsync()
        {
            if (_channel is not null) await _channel.CloseAsync();
            if (_connection is not null) await _connection.CloseAsync();
        }
    }
}
