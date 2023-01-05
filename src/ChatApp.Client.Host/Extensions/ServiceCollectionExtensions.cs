using ChatApp.Client.Contracts;
using ChatApp.Client.Initialization;
using ChatApp.Client.Initialization.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Serilog;

namespace ChatApp.Client.Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterCompositionRoot(this IServiceCollection services)
    {
        services
            .AddSingleton(sp =>
            {
                var clientConfiguration = sp.GetService<IOptions<ClientConfiguration>>()?.Value;

                var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

                var keyDeserializer = sp.GetService<IDeserializer<string>>();
                var messageValueDeserializer = sp.GetService<IDeserializer<Message>>();

                return GlobalDependency.Create(logger, clientConfiguration, keyDeserializer, messageValueDeserializer);
            })
            .AddSingleton(sp =>
            {
                var globalDependency = sp.GetRequiredService<GlobalDependency>();

                return new CompositionRoot(globalDependency);
            });

        return services;
    }
}
