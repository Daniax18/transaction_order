using Microsoft.EntityFrameworkCore;
using Minio;
using TransactionService.Application.Port.Inbound;
using TransactionService.Application.Port.Outbound;
using TransactionService.Application.UseCase;
using TransactionService.Infrastructure.Adapter.Outbound.Http;
using TransactionService.Infrastructure.Adapter.Outbound.Messaging;
using TransactionService.Infrastructure.Adapter.Outbound.Persistence.Repositories;
using TransactionService.Infrastructure.Adapter.Outbound.Storage.MinioAdapter;

namespace TransactionService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<Adapter.Outbound.Persistence.AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DockerDb")));

            // TODO : Add the endpoint in a configuration file
            services.AddHttpClient<IUserService, UserHttpClient>(client =>
            {
                client.BaseAddress = new Uri("http://auth-service:8081/api/user/"); // nom du service docker
            });

            // Register use case
            services.AddScoped<ICreateTransactionUseCase, CreateTransactionUseCase>();
            services.AddScoped<IVerifyTransactionUseCase, VerifyTransactionUseCase>();
            services.AddScoped<IGetTransactionUseCase, GetTransactionUseCase>();
            services.AddScoped<IUpdateStatusUseCase, UpdateStatusUseCase>();

            // Register Repositories
            services.AddScoped<ITransactionPersistence, TransactionRepository>();
            services.AddScoped<IMediaPersistence, MediaRepository>();

            // Register Storage Adapters
            var options = configuration
                .GetSection(MinioOptions.Section)
                .Get<MinioOptions>()!;
            services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.Section));
            services.AddSingleton<IMinioClient>(
                new MinioClient()
                    .WithEndpoint(options.Endpoint)
                    .WithCredentials(options.AccessKey, options.SecretKey)
                    //.WithSSL() // Decoment and implement on production if needed  
                    .Build()
            );

            services.AddScoped<IVideoStorage, MinioStorageAdapter>();

            services.AddSingleton<RabbitMQPublisher>();
            services.AddScoped<ILogTransactionService, LogTransactionService>();
            

            return services;
        }
    }
}
