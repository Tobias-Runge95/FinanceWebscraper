using WebScraper.Database;
using WebScraper.Database.Models;

namespace WebScrapper.Core;

public interface ITradeRepository
{
    Task CreateTrade(Trade trade);
    Task CreateMultipleTrades(List<Trade> trades);
    IQueryable<Trade> GetSearchableTrades();
}

public class TradeRepository : ITradeRepository
{
    private readonly WebScrapperDbContext _webScrapperDbContext;

    public TradeRepository(WebScrapperDbContext webScrapperDbContext)
    {
        _webScrapperDbContext = webScrapperDbContext;
    }

    public async Task CreateTrade(Trade trade)
    {
        await _webScrapperDbContext.Trades.AddAsync(trade);
        await _webScrapperDbContext.SaveChangesAsync();
    }

    public async Task CreateMultipleTrades(List<Trade> trades)
    {
        await _webScrapperDbContext.Trades.AddRangeAsync(trades);
        await _webScrapperDbContext.SaveChangesAsync();
    }

    public IQueryable<Trade> GetSearchableTrades()
        => _webScrapperDbContext.Trades.AsQueryable();
}