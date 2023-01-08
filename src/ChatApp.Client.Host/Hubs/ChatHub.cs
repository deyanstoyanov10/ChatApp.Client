namespace ChatApp.Client.Host.Hubs;

using ChatApp.Client.Contracts;
using ChatApp.Client.Core;
using ChatApp.Client.Handlers;
using ChatApp.Client.Initialization;
using ChatApp.Client.Initialization.Configuration;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly RequestHandler _requestHandler;
    private readonly ILogger<ChatHub> _logger;
    private readonly IObservable<Message> _stream;

    public ChatHub(CompositionRoot compositionRoot, GlobalDependency globalDependency, ILogger<ChatHub> logger)
    {
        _logger = logger;

        var eventStream = compositionRoot.Services.CommandHandler.Stream;

        _stream = eventStream;
        _stream.Subscribe(this.LogData);

        var requestHandler = RequestHandler.Create(eventStream, this.PushCallback(), globalDependency.PushClientConfiguration);

        _requestHandler = requestHandler;
    }

    private void LogData(Message message)
    {
        _logger.LogInformation($"Message Received command handler stream {message.Id}");
    }

    private Func<ClientResult, Task> PushCallback()
    {
        return clientResult =>
        {
            var response = ResponseHelpers.convertToResponseObject(clientResult);

            return Clients.All.SendAsync("ReceiveMessages", response);
        };
    }
}
