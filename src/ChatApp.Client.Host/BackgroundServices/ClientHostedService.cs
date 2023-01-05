namespace ChatApp.Client.Host.BackgroundServices;

using ChatApp.Client.Initialization;

public class ClientHostedService : IHostedService
{
    private readonly CompositionRoot _compositionRoot;

    public ClientHostedService(CompositionRoot compositionRoot) => _compositionRoot = compositionRoot;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _compositionRoot.Start();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _compositionRoot.Stop();

        return Task.CompletedTask;
    }
}
