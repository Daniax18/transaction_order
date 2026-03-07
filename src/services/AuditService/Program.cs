using AuditService;
using AuditService.Application.Interfaces;
using AuditService.Application.UseCases;
using AuditService.Infrastructure.Messaging;
using AuditService.Infrastructure.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<RabbitMQConsumer>();
builder.Services.AddScoped<ProcessAuditUseCase>();

builder.Services.AddScoped<IAuditProcessor, AuditProcessor>();  

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
