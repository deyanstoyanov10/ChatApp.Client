using ChatApp.Client.Host.Extensions;
using ChatApp.Client.Host.BackgroundServices;
using ChatApp.Client.Initialization.Configuration;
using ChatApp.Client.Infrastructure.KafkaConsumer;

using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<ClientConfiguration>(builder.Configuration.GetSection(nameof(ClientConfiguration)))
    .AddSingleton(typeof(IDeserializer<>), typeof(MessagePackDeserializer<>))
    .RegisterCompositionRoot()
    .AddHostedService<ClientHostedService>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
