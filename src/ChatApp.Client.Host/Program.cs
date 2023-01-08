using ChatApp.Client.Host.Extensions;
using ChatApp.Client.Host.BackgroundServices;
using ChatApp.Client.Initialization.Configuration;
using ChatApp.Client.Infrastructure.KafkaConsumer;

using Confluent.Kafka;
using ChatApp.Client.Host.Hubs;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<ClientConfiguration>(builder.Configuration.GetSection(nameof(ClientConfiguration)))
    .AddSingleton(typeof(IDeserializer<>), typeof(MessagePackDeserializer<>))
    .RegisterCompositionRoot()
    .AddHostedService<ClientHostedService>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddSignalR().Services
    .AddSingleton(typeof(ChatHub))
    .AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MySignalRPolicy", config =>
    {
        config.WithOrigins("https://localhost:7213/");
        config.AllowAnyMethod();
        config.AllowAnyHeader();
        config.SetIsOriginAllowed((x) => true);
        config.AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MySignalRPolicy");
app.MapHub<ChatHub>("/Websocket");

app.MapControllers();

app.Run();
