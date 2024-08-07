using Microsoft.Extensions.DependencyInjection;
using WebScrapper.Core.Services;

namespace WebScrapper.Core;

public static class Startup
{
    public static IServiceCollection Register(this IServiceCollection service)
    {
        return service
            .RegisterRepositories()
            .AddScoped<Writer>()
            .AddScoped<CapitolTradesScraper>();
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection service)
    {
        return service
            .AddScoped<ITraderRepository, TraderRepository>()
            .AddScoped<ITradeRepository, TradeRepository>()
            .AddScoped<IScrapingInfoRepository, ScrapingInfoRepository>();
    }
}