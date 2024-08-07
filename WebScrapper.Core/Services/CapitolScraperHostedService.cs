using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebScrapper.Core.Services;

public class CapitolScraperHostedService : IHostedService, IDisposable
{
    private Timer? _timer = null;
    private readonly IServiceProvider _serviceProvider;

    public CapitolScraperHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        
        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var serviceCTP = scope.ServiceProvider.GetRequiredService<CapitolTradesScraper>();
            await serviceCTP.StartScraping();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}