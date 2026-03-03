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
    public class RabbitMQConsumer : IAsyncDisposable
    {
        private readonly ProcessAuditUseCase _processAuditUseCase;
        private IConnection? _connection;
        private IChannel? _channel;
        private const string QueueName = "event.created";

        public RabbitMQConsumer(ProcessAuditUseCase processAuditUseCase)
        {
            _processAuditUseCase = processAuditUseCase;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await _channel.QueueDeclareAsync(
                queue: QueueName, 
                durable: true, 
                exclusive: false, 
                autoDelete: false, 
                cancellationToken: cancellationToken
            );
            await _channel.BasicQosAsync(
                 prefetchSize: 0,
                 prefetchCount: 1,
                 global: false,
                 cancellationToken: cancellationToken
            );
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel!);

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
                }
                catch (Exception ex)
                {
                    await _channel!.BasicNackAsync(
                        deliveryTag: args.DeliveryTag,
                        multiple: false,
                        requeue: true
                    );
                }
            };

            await _channel!.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken
            );

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
               
        public async ValueTask DisposeAsync()
        {
            if (_channel is not null) await _channel.CloseAsync();
            if (_connection is not null) await _connection.CloseAsync();
        }
    }
}
