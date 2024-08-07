using Microsoft.EntityFrameworkCore;
using WebScraper.Database;
using WebScraper.Database.Models;

namespace WebScrapper.Core;

public interface ITraderRepository
{
    Task CreateTrader(Trader trader);
    Task CreateTraders(List<Trader> traders);
    Task<Trader> GetTrader(Guid id);
    Task<Trader> GetTrader(string name);
    Task<List<Trader>> GetAllTraders();
}

public class TraderRepository : ITraderRepository
{
    private readonly WebScrapperDbContext _webScrapperDbContext;

    public TraderRepository(WebScrapperDbContext webScrapperDbContext)
    {
        _webScrapperDbContext = webScrapperDbContext;
    }

    public async Task CreateTrader(Trader trader)
    {
        await _webScrapperDbContext.Traders.AddAsync(trader);
        await _webScrapperDbContext.SaveChangesAsync();
    }

    public async Task CreateTraders(List<Trader> traders)
    {
        await _webScrapperDbContext.AddRangeAsync(traders);
        await _webScrapperDbContext.SaveChangesAsync();
    }

    public async Task<Trader> GetTrader(Guid id) => (await _webScrapperDbContext.Traders.AsQueryable()
        .Include(x => x.History)
        .FirstOrDefaultAsync(x => x.Id == id))!;

    public async Task<Trader> GetTrader(string name)=> (await _webScrapperDbContext.Traders.AsQueryable()
        .Include(x => x.History)
        .FirstOrDefaultAsync(x => x.Name == name))!;

    public async Task<List<Trader>> GetAllTraders() => await _webScrapperDbContext.Traders.ToListAsync();
}